using System;
using UnityEngine;

namespace BaCon
{
    public interface IInstanceInjector
    {
        T InstantiateAndResolve<T>(GameObject prefab, string tag = null);
        T InstantiateAndResolve<T>(T prefab, string tag = null) where T : MonoBehaviour;
        bool HasInjectionMethod(int key);
        void BindInjectionMethod<T>(Action<IDIResolver, T> method);
        void BindInjectionMethod<T>(string tag, Action<IDIResolver, T> method);
        T ResolveForInstance<T>(T instance, string tag = null);
    }
}