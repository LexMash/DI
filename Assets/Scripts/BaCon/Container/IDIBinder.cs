using System;

namespace BaCon
{
    public interface IDIBinder : IDIMethodBinder
    {
        DIEntryBuilder<TCurrent> Bind<TCurrent>(Func<IDIResolver, TCurrent> factory = null) where TCurrent : new();
        DIEntryBuilder<TCurrent, TTarget> Bind<TCurrent, TTarget>(Func<IDIResolver, TCurrent> factory = null) where TCurrent : TTarget, new();
        DIEntryBuilder<TCurrent> BindInstance<TCurrent>(TCurrent instance);
        DIEntryBuilder<TCurrent, TTarget> BindInstance<TCurrent, TTarget>(TCurrent instance) where TCurrent : TTarget;
    }
}