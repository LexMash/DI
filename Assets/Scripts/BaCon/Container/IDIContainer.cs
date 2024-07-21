using System;

namespace BaCon
{
    public interface IDIContainer : IInstanceInjector
    {
        DIEntryBuilder<TCurrent> Register<TCurrent>(Func<DIContainer, TCurrent> factory = null) where TCurrent : new();
        DIEntryBuilder<TCurrent, TTarget> Register<TCurrent, TTarget>(Func<DIContainer, TCurrent> factory = null) where TCurrent : TTarget, new();
        public DIEntryBuilder<TCurrent> RegisterInstance<TCurrent>(TCurrent instance);
        DIEntryBuilder<TCurrent, TTarget> RegisterInstance<TCurrent, TTarget>(TCurrent instance) where TCurrent : TTarget;
        T Resolve<T>(string tag = null);
    }
}