namespace BaCon
{
    public class DIResolverDecorator<TTarget, TCurrent> : DIResolver<TTarget> where TCurrent : TTarget
    {
        private readonly DIResolver<TCurrent> decorated;

        public DIResolverDecorator(DIResolver<TCurrent> decorated)
        {
            this.decorated = decorated;
        }

        public override TTarget Resolve()
        {
            return decorated.Resolve();
        }

        public override void NonLazy()
        {
            decorated.NonLazy();
        }
    }
}