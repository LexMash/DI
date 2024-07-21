using System;

namespace BaCon
{
    public class ActionDIEntry<T> : DIResolver<T>
    {
        private readonly DIContainer container;
        private readonly Action<DIContainer, T> injectionAction;
        private readonly DIEntry<T> entry;
        private bool isResolved;

        public ActionDIEntry(DIContainer container, Action<DIContainer, T> injectionAction, DIEntry<T> entry)
        {
            this.container = container;
            this.injectionAction = injectionAction;
            this.entry = entry;
        }

        public override T Resolve()
        {
            var resolved = entry.Resolve();

            if (entry.IsSingle)
            {
                if (!isResolved)
                {
                    injectionAction(container, resolved);
                    isResolved = true;                  
                }

                return resolved;
            }

            injectionAction(container, resolved);
            return resolved;
        }

        public override void NonLazy()
        {
            if (entry.IsSingle)
            {
                entry.NonLazy();
                var resolved = entry.Resolve();
                injectionAction(container, resolved);
                isResolved = true;
            }
        }
    }
}
