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
        protected DIContainer Container { get; }
        
        public T Resolve<T>()
        {
            return ((DIEntryResolver<T>)this).Resolve();
        }
    }

    public class DIEntry<T> : DIEntryResolver<T>
    {
        private Func<DIContainer, T> Factory { get; }
        private T _instance;

        public DIEntry(DIContainer container, Func<DIContainer, T> factory, bool isSingle = false)
        {
            Factory = factory;
            IsSingle = isSingle;

            RegisteredType = typeof(T);
        }

        public DIEntry(T value)
        {
            _instance = value;
            IsSingle = true;
        }

        public override T Resolve()
        {
            if (IsSingle)
            {
                return _instance ??= Factory(Container);
            }

            return Factory(Container);
        }

        public override void NonLazy()
        {
            if (IsSingle)
            {
                _instance = _instance == null ? Factory(Container) : _instance;
            }
        }
    }
}