using System;

namespace BaCon
{
    public abstract class MethodResolver
    {
        public void Resolve<T>(T instance)
        {
            ((MethodResolver<T>)this).Resolve(instance);
        }
    }

    public sealed class MethodResolver<T> : MethodResolver
    {
        private readonly IDIResolver resolver;
        private readonly Action<IDIResolver, T> process;

        public MethodResolver(IDIResolver resolver, Action<IDIResolver, T> process)
        {
            this.resolver = resolver;
            this.process = process;
        }

        public void Resolve(T instance)
        {
            process(resolver, instance);
        }
    }
}