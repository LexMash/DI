using System;

namespace BaCon
{
    public interface IDIContainer
    {
        DIEntryBuilder<TCurrent, TTarget> Register<TCurrent, TTarget>(Func<DIContainer, TCurrent> factory = null) where TCurrent : TTarget, new();
        DIEntryBuilder<TCurrent, TTarget> RegisterInstance<TCurrent, TTarget>(TCurrent instance) where TCurrent : TTarget;
        T Resolve<T>(string tag = null);
    }
}