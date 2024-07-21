﻿using System;

namespace BaCon
{
    public abstract class DIEntryBuilder<TCurrent>
    {
        protected readonly DIContainer Container;
        protected string Tag;
        protected bool IsSingle;
        protected bool CreateAfterBindings;
        protected Action<DIContainer, TCurrent> InjectAction;
        protected DIResolver<TCurrent> Resolver;

        protected DIEntryBuilder(DIContainer container)
        {
            Container = container;
        }

        public DIEntryBuilder<TCurrent> WithInjectionAction(Action<DIContainer, TCurrent> injectAction)
        {
            InjectAction = injectAction;
            return this;
        }

        public DIEntryBuilder<TCurrent> AsSingle()
        {
            IsSingle = true;
            return this;
        }

        public DIEntryBuilder<TCurrent> WithTag(string tag)
        {
            Tag = tag;
            return this;
        }

        public DIEntryBuilder<TCurrent> NonLazy()
        {
            CreateAfterBindings = true;
            return this;
        }

        public void Bind()
        {
            bool hasPostInjectionAction = InjectAction != null;

            if (hasPostInjectionAction)
                Container.RegisterInjectionMethod(Tag, InjectAction);

            var entry = GetEntry();

            Resolver = !hasPostInjectionAction
                ? entry
                : new PostresolvedDIEntry<TCurrent>(Container, entry, Tag);

            var key = DIContainer.GetKey<TCurrent>(Tag);
            Container.RegisterEntry(Resolver, key, CreateAfterBindings);
        }

        protected abstract DIEntry<TCurrent> GetEntry();
    }

    public abstract class DIEntryBuilder<TCurrent, TTarget> where TCurrent : TTarget
    {
        protected readonly DIContainer Container;
        protected string Tag;
        protected bool IsSingle;
        protected bool CreateAfterBindings;
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
            CreateAfterBindings = true;
            return this;
        }

        public void Bind()
        {                        
            bool hasPostInjectionAction = InjectAction != null;
            
            if (hasPostInjectionAction)
                Container.RegisterInjectionMethod(Tag, InjectAction);

            var entry = GetEntry();

            Resolver = !hasPostInjectionAction
                ? new DIResolverAdapter<TTarget, TCurrent>(entry)
                : new DIResolverAdapter<TTarget, TCurrent>(new PostresolvedDIEntry<TCurrent>(Container, entry, Tag));

            var key = DIContainer.GetKey<TTarget>(Tag);
            Container.RegisterEntry(Resolver, key, CreateAfterBindings);      
        }

        protected abstract DIEntry<TCurrent> GetEntry();
    }
}
