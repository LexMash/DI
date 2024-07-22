using System;

namespace BaCon
{
    public sealed class DIFactoryBuilder<TCurrent> : DIEntryBuilder<TCurrent> where TCurrent : new()
    {
        private readonly Func<IDIResolver, TCurrent> factory;

        public DIFactoryBuilder(DIContainer container, Func<IDIResolver, TCurrent> factory) : base(container)
        {
            this.factory = factory ??= (c) => new();
        }

        protected override DIEntry<TCurrent> GetEntry()
            => new DIEntry<TCurrent>(Container, factory, IsSingle);
    }


    public sealed class DIFactoryBuilder<TCurrent, TTarget> : DIEntryBuilder<TCurrent, TTarget> where TCurrent : TTarget, new()
    {
        private readonly Func<IDIResolver, TCurrent> factory;

        public DIFactoryBuilder(DIContainer container, Func<IDIResolver, TCurrent> factory) : base(container)
        {
            this.factory = factory ??= (c) => new();
        }

        protected override DIEntry<TCurrent> GetEntry() 
            => new DIEntry<TCurrent>(Container, factory, IsSingle);
    }
}