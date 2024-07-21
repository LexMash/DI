using System;

namespace BaCon
{
    public abstract class DIEntry : Lazy
    {
        public Type RegisteredType { get; protected set; }
        public bool IsSingle { get; protected set; }
        protected DIContainer Container { get; }
        
        public T Resolve<T>()
        {
            return ((DIResolver<T>)this).Resolve();
        }
    }

    public class DIEntry<T> : DIResolver<T>
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