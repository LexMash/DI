using System;

namespace BaCon
{
    public class DIFactoryBuilder<TCurrent, TTarget> : DIEntryBuilder<TCurrent, TTarget> where TCurrent : TTarget, new()
    {
        private readonly Func<DIContainer, TCurrent> factory;

        public DIFactoryBuilder(DIContainer container, Func<DIContainer, TCurrent> factory) : base(container)
        {
            this.factory = factory ??= (c) => new();
        }

        protected override DIEntry<TCurrent> GetEntry() 
            => new DIEntry<TCurrent>(Container, factory, IsSingle);
    }
}