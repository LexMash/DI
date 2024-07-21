using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaCon
{
    public class DIContainer : IDIContainer
    {
        private readonly DIContainer _parentContainer;
        private readonly Dictionary<(string, Type), DIEntry> _entriesMap = new();
        private readonly HashSet<(string, Type)> _resolutionsCache = new();
        private readonly Dictionary<(string, Type), MethodResolver> _resolverMap = new();
        private readonly Stack<Lazy> _lazyStack = new();

        private int registrationRequeststrationCounter;

        public DIContainer(DIContainer parentContainer = null)
        {
            _parentContainer = parentContainer;
        }

        public DIEntryBuilder<TCurrent, TTarget> Register<TCurrent, TTarget>(Func<DIContainer, TCurrent> factory = null) where TCurrent : TTarget, new()
        {
            registrationRequeststrationCounter++;
            return new DIFactoryBuilder<TCurrent, TTarget>(this, factory);
        }

        public DIEntryBuilder<TCurrent, TTarget> RegisterInstance<TCurrent, TTarget>(TCurrent instance) where TCurrent : TTarget
        {
            registrationRequeststrationCounter++;
            return new DIInstanceBuilder<TCurrent, TTarget>(this, instance);
        }

        public void RegisterInjectionMethod<T>(string tag, Action<DIContainer, T> method)
        {
            var key = GetKey<T>(tag);

            if (HasResolver(key))
            {
                Debug.LogError($"Resolvers already contains resolver with tag {key.Item1} and type {key.Item2.FullName}");
                return;
            }

            _resolverMap[key] = new MethodResolver<T>(this, method);
        }

        public T Resolve<T>(string tag = null)
        {
            if (!BindsCompleted())
                Debug.LogWarning("Not all registration requests have been completed");

            var key = (tag, typeof(T));

            if (_resolutionsCache.Contains(key))
                throw new Exception($"DI: Cyclic dependency for tag {key.tag} and type {key.Item2.FullName}");

            _resolutionsCache.Add(key);

            try
            {
                if (_entriesMap.TryGetValue(key, out var diEntry))
                    return diEntry.Resolve<T>();

                if (_parentContainer != null)
                    return _parentContainer.Resolve<T>(tag);
            }
            finally
            {
                _resolutionsCache.Remove(key);
            }

            throw new Exception($"Couldn't find dependency for tag {tag} and type {key.Item2.FullName}");
        }

        public T ResolveForInstance<T>(T instance, string tag = null)
        {
            var key = GetKey<T>(tag);

            MethodResolver<T> resolver = GetResolver<T>(key);
            resolver.Resolve(instance);
            return instance;
        }

        public void RegisterEntry(DIEntry entry, (string, Type) key, bool nonLazy = false)
        {
            EntriesContainsCheck(key);

            _entriesMap[key] = entry;

            if (nonLazy)
                _lazyStack.Push(entry);

            registrationRequeststrationCounter--;

            if (BindsCompleted())
                RegistrationComplete();
        }

        private void RegistrationComplete()
        {
            while(_lazyStack.Count > 0)
            {
                var entry = _lazyStack.Pop();
                entry.NonLazy();
            }

            registrationRequeststrationCounter = 0;
        }

        public bool HasResolver((string tag, Type) key)
        {
            return _resolverMap.ContainsKey(key);
        }

        public static (string tag, Type) GetKey<T>(string tag) => (tag, typeof(T));

        private bool BindsCompleted() => registrationRequeststrationCounter == 0;

        private void EntriesContainsCheck((string tag, Type) key)
        {
            if (_entriesMap.TryGetValue(key, out DIEntry existed))
                throw new Exception($"Entry with this key {key.Item1} {key.Item2.Name}");
        }

        private MethodResolver<T> GetResolver<T>((string, Type) key)
        {
            MethodResolver<T> resolver = null;

            if (_resolverMap.TryGetValue(key, out MethodResolver registered))
            {
                resolver = (MethodResolver<T>)registered;
            }
            else
                throw new Exception($"Couldn't find resolver for tag {key.Item1} and type {key.Item2.FullName}");

            return resolver;
        }
    }
}