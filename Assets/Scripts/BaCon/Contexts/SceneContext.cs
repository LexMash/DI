using UnityEngine;

namespace BaCon
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private DIRegistrator[] registrators;

        private DIContainer container;

        public void InitContext(DIContainer container)
        {
            container = new DIContainer(container);

            var count = registrators.Length;
            for (int i = 0; i < count; i++)
            {
                DIRegistrator registrator = registrators[i];
                registrator.RegisterEntries(container);
            }

            container.CompleteRegistration();
        }

        private void OnDestroy()
        {
            container?.Dispose();
        }
    }
}
