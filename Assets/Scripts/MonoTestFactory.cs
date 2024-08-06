using BaCon;
using UnityEngine;

namespace Assets.Scripts
{
    public class MonoTestFactory : MonoBehaviour
    {
        [SerializeField] private MonoTestObject prefab;

        private IDIResolver resolver;

        public void Construct(IDIResolver resolver)
        {
            this.resolver = resolver;

            Create();
        }

        public MonoTestObject Create()
        {
            MonoTestObject instance = Instantiate(prefab);
            resolver.ResolveAllHierarchy(instance);
            return instance;
        }
    }
}
