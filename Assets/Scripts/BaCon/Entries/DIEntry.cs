using System;

namespace BaCon
{

#if UNITY_2017_3_OR_NEWER && NET_4_6
    /// <summary>
    /// Interface ONLY for deriving MonoBehaviour classes and injecting into its entire hierarchy.
    /// </summary>
    public interface IInjectable
    {
        public string Tag { get; }
    }
#endif

    public abstract class DIEntry : Lazy
    {
        public Type RegisteredType { get; protected set; }
        public bool IsSingle { get; protected set; }
        
        public T Resolve<T>()
        {
            return ((DIEntryResolver<T>)this).Resolve();
        }
    }

    public class DIEntry<T> : DIEntryResolver<T>
    {
        private readonly IDIResolver resolver;
        private readonly Func<IDIResolver, T> factory;
        private T instance;

        public DIEntry(IDIResolver resolver, Func<IDIResolver, T> factory, bool isSingle = false)
        {
            this.resolver = resolver;
            this.factory = factory;
            IsSingle = isSingle;

            RegisteredType = typeof(T);
        }

        public DIEntry(T instance)
        {
            this.instance = instance;
            IsSingle = true;
        }

        public override T Resolve()
        {
            if (IsSingle)
            {
                if(instance == null)
                {
                    instance ??= factory(resolver);
                    resolver.TryAddToDisposable(instance);
                }

                return instance;
            }

            return factory(resolver);
        }

        public override void NonLazy()
        {
            instance = instance == null ? factory(resolver) : instance;
            resolver.TryAddToDisposable(instance);
        }
    }
}