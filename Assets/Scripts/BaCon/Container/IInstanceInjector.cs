using System;
using UnityEngine;

namespace BaCon
{
    public interface IInstanceInjector
    {
        T InstantiateAndResolve<T>(GameObject prefab, string tag = null);
        T InstantiateAndResolve<T>(T prefab, string tag = null) where T : MonoBehaviour;
        bool HasInjectionMethod(int key);
        void RegisterInjectionMethod<T>(Action<DIContainer, T> method);
        void RegisterInjectionMethod<T>(string tag, Action<DIContainer, T> method);
        T ResolveForInstance<T>(T instance, string tag = null);
    }
}