using System.Collections.Generic;
using UnityEngine;

namespace BaCon
{
    public interface IDIResolver
    {
        T Resolve<T>(string tag = null);
        IReadOnlyList<T> ResolveAll<T>();
        T InstantiateAndResolve<T>(GameObject prefab, string tag = null);
        T InstantiateAndResolve<T>(T prefab, string tag = null) where T : MonoBehaviour;
    }
}