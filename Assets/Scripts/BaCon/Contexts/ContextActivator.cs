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
            context?.OnSceneLoaded(scene, mode);
        }

        private static ProjectContext CreateContext()
        {
            var contextPrefab = Resources.Load<ProjectContext>(ProjectContextLoadPath);

            if (contextPrefab == null)
            {
                Debug.LogError("The project context could not be created.Check that the Resources folder contains the ProjectContext prefab");
                return null;
            }
            
            return GameObject.Instantiate(contextPrefab);
        }
    }
}
