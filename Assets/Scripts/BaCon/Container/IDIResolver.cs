using UnityEngine;

namespace BaCon
{
    public interface IDIResolver
    {
        T Resolve<T>(string tag = null);
        T InstantiateAndResolve<T>(GameObject prefab, string tag = null);
        T InstantiateAndResolve<T>(T prefab, string tag = null) where T : MonoBehaviour;
        T ResolveForInstance<T>(T instance, string tag = null);
    }
}