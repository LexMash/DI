namespace BaCon
{
    public sealed class DIEntryPostresolved<T> : DIEntryResolver<T>
    {
        private readonly IDIResolver resolver;
        private readonly DIEntry<T> entry;
        private readonly string tag;
        private bool postInjectionPerformed;

        public DIEntryPostresolved(IDIResolver resolver, DIEntry<T> entry, string tag)
        {
            this.resolver = resolver;
            this.entry = entry;
            this.tag = tag;
            IsSingle = entry.IsSingle;
            RegisteredType = typeof(T);
        }

        public override T Resolve()
        {
            var instance = entry.Resolve();

            if (entry.IsSingle)
            {
                if (!postInjectionPerformed)
                {
                    resolver.ResolveForInstance(instance, tag);
                    postInjectionPerformed = true;                  
                }

                return instance;
            }

            resolver.ResolveForInstance(instance, tag);
            return instance;
        }

        public override void NonLazy()
        {
            entry.NonLazy();
            var instance = entry.Resolve();
            resolver.ResolveForInstance(instance, tag);
            postInjectionPerformed = true;
        }
    }
}
