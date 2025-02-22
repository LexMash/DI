﻿using System.Collections.Generic;
using UnityEngine;

namespace BaCon
{
    public interface IDIResolver
    {
        T Resolve<T>(string tag = null);
        IReadOnlyList<T> ResolveAll<T>();
        T InstantiateAndResolve<T>(GameObject prefab, string tag = null);
        T InstantiateAndResolve<T>(T prefab, string tag = null) where T : MonoBehaviour;
        void ResolveForInstance<T>(T instance, string tag = null);

#if ENABLE_MONO && UNITY_2017_3_OR_NEWER && NET_4_6 
        void ResolveAllHierarchy<T>(T instance) where T : MonoBehaviour;
        void ResolveAllHierarchy(GameObject gameObject);
#endif
        bool TryAddToDisposable(object instance); //we need to come up with something about this, this shouldn't be here
    }
}