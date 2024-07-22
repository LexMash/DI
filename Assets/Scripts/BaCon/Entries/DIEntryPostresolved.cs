namespace BaCon
{
    public sealed class DIEntryPostresolved<T> : DIEntryResolver<T>
    {
        private readonly DIContainer container;
        private readonly DIEntry<T> entry;
        private readonly string tag;
        private bool isResolved;

        public DIEntryPostresolved(DIContainer container, DIEntry<T> entry, string tag)
        {
            this.container = container;
            this.entry = entry;
            this.tag = tag;

            RegisteredType = entry.RegisteredType;
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
