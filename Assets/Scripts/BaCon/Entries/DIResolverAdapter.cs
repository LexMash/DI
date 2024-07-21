namespace BaCon
{
    public sealed class DIResolverAdapter<TTarget, TCurrent> : DIResolver<TTarget> where TCurrent : TTarget
    {
        private readonly DIResolver<TCurrent> resolver;

        public DIResolverAdapter(DIResolver<TCurrent> resolver)
        {
            this.resolver = resolver;

            RegisteredType = resolver.RegisteredType;
        }

        public override TTarget Resolve()
        {
            return resolver.Resolve();
        }

        public override void NonLazy()
        {
            resolver.NonLazy();
        }
    }
}