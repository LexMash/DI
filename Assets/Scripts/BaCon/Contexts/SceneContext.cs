using UnityEngine;

namespace BaCon
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private DIRegistrator[] registrators;

        private DIContainer container;

        public void InitContext(DIContainer container)
        {
            this.container = new DIContainer(container);

            var count = registrators.Length;
            for (int i = 0; i < count; i++)
            {
                DIRegistrator registrator = registrators[i];
                registrator.BindEntries(container);
            }

            container.BuildDomain();
        }

        private void OnDestroy()
        {
            container?.Dispose();
        }
    }
}
