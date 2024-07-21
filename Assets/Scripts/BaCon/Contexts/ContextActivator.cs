using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaCon
{
    public static class ContextActivator
    {
        private const string ProjectContextLoadPath = "ProjectContext";
        private static ProjectContext context;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void WaitSceneLoaded()
        {
            SceneManager.sceneLoaded += InitializeContext;
        }

        private static void InitializeContext(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= InitializeContext;
            context = CreateContext();
            context.OnSceneLoaded(scene, mode);
        }

        private static ProjectContext CreateContext()
        {
            var contextPrefab = Resources.Load<ProjectContext>(ProjectContextLoadPath);
            return GameObject.Instantiate(contextPrefab);
        }
    }
}
