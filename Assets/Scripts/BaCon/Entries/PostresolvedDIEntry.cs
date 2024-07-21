namespace BaCon
{
    public sealed class PostresolvedDIEntry<T> : DIResolver<T>
    {
        private readonly DIContainer container;
        private readonly DIEntry<T> entry;
        private readonly string tag;
        private bool isResolved;

        public PostresolvedDIEntry(DIContainer container, DIEntry<T> entry, string tag)
        {
            this.container = container;
            this.entry = entry;
            this.tag = tag;
        }

        public override T Resolve()
        {
            var resolved = entry.Resolve();

            if (entry.IsSingle)
            {
                if (!isResolved)
                {
                    container.ResolveForInstance(resolved, tag);
                    isResolved = true;                  
                }

                return resolved;
            }

            container.ResolveForInstance(resolved, tag);
            return resolved;
        }

        public override void NonLazy()
        {
            if (entry.IsSingle)
            {
                entry.NonLazy();
                var resolved = entry.Resolve();
                container.ResolveForInstance(resolved, tag);
                isResolved = true;
            }
        }
    }
}
