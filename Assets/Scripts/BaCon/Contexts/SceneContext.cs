using UnityEngine;

namespace BaCon
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private DIInstaller[] registrators;

        private DIContainer container;

        public void InitContext(DIContainer container)
        {
            this.container = new DIContainer(container);

            var count = registrators.Length;
            for (int i = 0; i < count; i++)
            {
                DIInstaller registrator = registrators[i];
                registrator.InstallBindings(container);
            }

            container.BuildContext();
        }

        private void OnDestroy()
        {
            container?.Dispose();
        }
    }
}
