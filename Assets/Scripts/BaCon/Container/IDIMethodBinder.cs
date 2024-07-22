using System;

namespace BaCon
{
    public interface IDIMethodBinder
    {
        void BindInjectionMethod<T>(Action<IDIResolver, T> method);
        void BindInjectionMethod<T>(string tag, Action<IDIResolver, T> method);       
    }
}