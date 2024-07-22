namespace BaCon
{
    public sealed class DIEntryResolverAdapter<TTarget, TCurrent> : DIEntryResolver<TTarget> where TCurrent : TTarget
    {
        private readonly DIEntryResolver<TCurrent> resolver;

        public DIEntryResolverAdapter(DIEntryResolver<TCurrent> resolver)
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