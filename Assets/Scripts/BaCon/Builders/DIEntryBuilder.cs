using System;

namespace BaCon
{
    public abstract class DIEntryBuilder<TCurrent, TTarget> where TCurrent : TTarget
    {
        protected readonly DIContainer Container;
        protected string Tag;
        protected bool IsSingle;
        protected bool NoLazy;
        protected Action<DIContainer, TCurrent> InjectAction;
        protected DIResolver<TTarget> Resolver;

        protected DIEntryBuilder(DIContainer container)
        {
            Container = container;
        }

        public DIEntryBuilder<TCurrent, TTarget> WithInjectionAction(Action<DIContainer, TCurrent> injectAction)
        {
            InjectAction = injectAction;
            return this;
        }

        public DIEntryBuilder<TCurrent, TTarget> AsSingle()
        {
            IsSingle = true;
            return this;
        }

        public DIEntryBuilder<TCurrent, TTarget> WithTag(string tag)
        {
            Tag = tag;
            return this;
        }

        public DIEntryBuilder<TCurrent, TTarget> NonLazy()
        {
            NoLazy = true;
            return this;
        }

        public void Bind()
        {                        
            bool hasPostInjectionAction = InjectAction != null;
            
            if (hasPostInjectionAction)
            {
                var currentKey = DIContainer.GetKey<TCurrent>(Tag);

                if (Container.HasResolver(currentKey) == false)
                    Container.RegisterInjectionMethod(Tag, InjectAction);
            }

            var entry = GetEntry();

            Resolver = !hasPostInjectionAction
                ? new DIResolverDecorator<TTarget, TCurrent>(entry)
                : new DIResolverDecorator<TTarget, TCurrent>
                (new ActionDIEntry<TCurrent>(Container, InjectAction, entry));

            var targetKey = DIContainer.GetKey<TTarget>(Tag);
            Container.RegisterEntry(Resolver, targetKey, NoLazy);      
        }

        protected abstract DIEntry<TCurrent> GetEntry();
    }
}
