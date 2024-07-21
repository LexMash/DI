using System;

namespace BaCon
{
    public abstract class MethodResolver
    {
        protected readonly DIContainer Container;

        public MethodResolver(DIContainer container)
        {
            Container = container;
        }

        public void Resolve<T>(T instance)
        {
            ((MethodResolver<T>)this).Resolve(instance);
        }
    }

    public class MethodResolver<T> : MethodResolver
    {
        private readonly Action<DIContainer, T> process;

        public MethodResolver(DIContainer container, Action<DIContainer, T> process) : base(container)
        {
            this.process = process;
        }

        public void Resolve(T instance)
        {
            process(Container, instance);
        }
    }
}