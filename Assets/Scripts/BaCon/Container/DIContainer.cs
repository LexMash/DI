using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaCon
{

    public class DIContainer : IDIBinder, IDIResolver, IDisposable
    {
        private readonly DIContainer _parentContainer;

        private readonly Dictionary<int, DIEntry> _entriesMap = new();
        private readonly Dictionary<int, MethodResolver> _resolverMap = new();
        private readonly Dictionary<Type, List<int>> _cashedKeysMap = new();
        private readonly Stack<IDisposable> _disposables = new();

        private readonly HashSet<int> _resolutionsCache = new();      
        private readonly Queue<Lazy> _lazyQueue = new();
        private Queue<IDIEntryBuilder> _buildersQueue = new();

        public DIContainer(DIContainer parentContainer = null)
        {
            _parentContainer = parentContainer;
        }

        public static int GetKey<T>(string tag)
            => HashCode.Combine(tag, typeof(T));

        public static int GetKey(string tag, Type type) 
            => HashCode.Combine(tag, type);

        public void RegisterEntry(DIEntry entry, int key, bool nonLazy, bool asCashed)
        {
            EntriesContainsCheck(key);

            _entriesMap[key] = entry;

            if (nonLazy)
                _lazyQueue.Enqueue(entry);

            if (asCashed)
                AddCashed(entry.RegisteredType, key);
        }

        public DIEntryBuilder<TCurrent> Bind<TCurrent>(Func<IDIResolver, TCurrent> factory = null) where TCurrent : new()
        {
            var builder = new DIFactoryBuilder<TCurrent>(this, factory);
            _buildersQueue.Enqueue(builder);
            return builder;
        }

        public DIEntryBuilder<TCurrent, TTarget> Bind<TCurrent, TTarget>(Func<IDIResolver, TCurrent> factory = null) where TCurrent : TTarget, new()
        {
            var builder = new DIFactoryBuilder<TCurrent, TTarget>(this, factory);
            _buildersQueue.Enqueue(builder);
            return builder;
        }

        public DIEntryBuilder<TCurrent> BindInstance<TCurrent>(TCurrent instance)
        {
            var builder = new DIInstanceBuilder<TCurrent>(this, instance);
            _buildersQueue.Enqueue(builder);
            return builder;
        }

        public DIEntryBuilder<TCurrent, TTarget> BindInstance<TCurrent, TTarget>(TCurrent instance) where TCurrent : TTarget
        {
            var builder = new DIInstanceBuilder<TCurrent, TTarget>(this, instance);
            _buildersQueue.Enqueue(builder);
            return builder;
        }

        public void BindInjectionMethod<T>(string tag, Action<IDIResolver, T> method)
        {
            var key = GetKey<T>(tag);

            if (HasInjectionMethod(key))
            {
                Debug.LogError($"Methods already contains method with tag {tag} and type {typeof(T).FullName}");
                return;
            }

            _resolverMap[key] = new MethodResolver<T>(this, method);
        }

        public void BindInjectionMethod<T>(Action<IDIResolver, T> method)
        {
            BindInjectionMethod(null, method);
        }

        public T Resolve<T>(string tag = null)
        {
            var key = GetKey<T>(tag);

            if (_resolutionsCache.Contains(key))
                throw new Exception($"DI: Cyclic dependency for tag {tag} and type {typeof(T).FullName}");

            _resolutionsCache.Add(key);

            try
            {
                if (_entriesMap.TryGetValue(key, out var diEntry))
                {
                    T resolved = diEntry.Resolve<T>();

                    if(diEntry.IsSingle)
                        TryAddToDisposable(resolved);

                    return resolved;
                }

                if (_parentContainer != null)
                    return _parentContainer.Resolve<T>(tag);
            }
            finally
            {
                _resolutionsCache.Remove(key);
            }

            throw new Exception($"Couldn't find dependency for tag {tag} and type {typeof(T).FullName}");
        }

        public IReadOnlyList<T> ResolveAll<T>()
        {
            var type = typeof(T);
            
            if (!_cashedKeysMap.TryGetValue(type, out List<int> cashedKeys))
                throw new TypeAccessException($"Collection of {type.FullName} not exists in cashed base");

            List<T> all = new();
            var count = cashedKeys.Count;

            for ( int i = 0; i < count; i++)
            {
                var key = cashedKeys[i];

                if (_resolutionsCache.Contains(key))
                    throw new Exception($"DI: Cyclic dependency for index {i} in cashed for type {type.FullName}. Check your cashed registrations");

                _resolutionsCache.Add(key);

                var entry = _entriesMap[key];
                var instance = entry.Resolve<T>();
                all.Add(instance);

                if (entry.IsSingle)
                    TryAddToDisposable(instance);

                _resolutionsCache.Remove(key);
            }

            if (_parentContainer != null) //или ограничиться текущим контекстом?
            {
                all.AddRange(_parentContainer.ResolveAll<T>());
                return all;
            }

            return all;
        }

        public T ResolveForInstance<T>(T instance, string tag = null)
        {
            var key = GetKey<T>(tag);

            MethodResolver<T> resolver = GetResolver<T>(key);
            resolver.Resolve(instance);
            return instance;
        }

#if UNITY_2017_3_OR_NEWER && NET_4_6
        public void ResolveAllHierarchy<T>(T instance) where T : MonoBehaviour
        {
            ResolveAllHierarchy(instance.gameObject);
        }

        public void ResolveAllHierarchy(GameObject gameObject)
        {
            var children = gameObject.GetComponentsInChildren<IInjectable>();
            var count = children.Length;

            for (int i = 0; i < count; i++)
            {
                IInjectable child = children[i];
                ResolveForInstance(child);
            }
        }
#endif

        public T InstantiateAndResolve<T>(GameObject prefab, string tag = null)
        {
            var instance = GameObject.Instantiate(prefab).GetComponent<T>();

            if (instance == null)
                throw new MissingComponentException($"Prefab {prefab.name} not contains component with type {typeof(T)}");

            return ResolveForInstance(instance, tag);
        }

        public T InstantiateAndResolve<T>(T prefab, string tag = null) where T : MonoBehaviour
        {
            var instance = GameObject.Instantiate(prefab);
            return ResolveForInstance(instance, tag);
        }

        public void BuildContext()
        {
            BindAll();
            NonLazy();
        }

        public void Dispose()
        {
            _entriesMap.Clear();
            _resolverMap.Clear();
            
            foreach(var cashed in _cashedKeysMap.Values)
                cashed.Clear();
            _cashedKeysMap.Clear();

            _lazyQueue.Clear();
            _resolutionsCache.Clear();

            var count = _disposables.Count;
            while(_disposables.Count > 0)
            {
                IDisposable disposable = _disposables.Pop();
                disposable.Dispose();
            }
            _disposables.Clear();
        }

        private bool HasInjectionMethod(int key) 
            => _resolverMap.ContainsKey(key);

        private void BindAll()
        {
            while (_buildersQueue.Count > 0)
            {
                var builder = _buildersQueue.Dequeue();
                builder.Bind();
            }

            _buildersQueue.Clear();
        }

        private void NonLazy()
        {
            while (_lazyQueue.Count > 0)
            {
                var entry = _lazyQueue.Dequeue();
                entry.NonLazy();
            }

            _lazyQueue.Clear();
        }

#if UNITY_2017_3_OR_NEWER && NET_4_6
        private void ResolveForInstance(IInjectable injectable)
        {
            ResolveForInstance((dynamic)injectable, injectable.Tag);
        }
#endif

        private bool TryAddToDisposable<T>(T instance)
        {
            var disposable = instance as IDisposable;

            if (disposable != null && !_disposables.Contains(disposable))
            {
                _disposables.Push(disposable);

                return true;
            }

            return false;
        }

        private void AddCashed(Type type, int key)
        {
            if (_cashedKeysMap.TryGetValue(type, out var keys))
            {
                //for test
                if (keys.Contains(key))
                    throw new InvalidOperationException($"Key {key} already registred in chased list for type {type.FullName}");

                keys.Add(key);
            }
            else
                _cashedKeysMap[type] = new List<int> { key };
        }

        private void EntriesContainsCheck(int key)
        {
            if (_entriesMap.TryGetValue(key, out DIEntry existed))
                throw new Exception($"Entry with this key {key} {existed.RegisteredType.Name}");
        }

        private MethodResolver<T> GetResolver<T>(int key)
        {
            MethodResolver<T> resolver = null;

            if (_resolverMap.TryGetValue(key, out MethodResolver registered))
            {
                resolver = (MethodResolver<T>)registered;
            }
            else
                throw new Exception($"Couldn't find resolver for tag {key} and type {typeof(T).FullName}");

            return resolver;
        }
    }
}