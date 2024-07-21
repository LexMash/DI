using System;

namespace BaCon
{
    public interface IDIContainer
    {
        DIEntryBuilder<TCurrent, TTarget> Register<TCurrent, TTarget>(Func<DIContainer, TCurrent> factory = null) where TCurrent : TTarget, new();
        DIEntryBuilder<TCurrent, TTarget> RegisterInstance<TCurrent, TTarget>(TCurrent instance) where TCurrent : TTarget;
        void RegisterInjectionMethod<T>(string tag, Action<DIContainer, T> method);  
        T Resolve<T>(string tag = null);
        T ResolveForInstance<T>(T instance, string tag = null);
    }
}