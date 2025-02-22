﻿using UnityEngine;
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

            CreateContext();
        }

        private static void CreateContext()
        {
            var contextPrefab = Resources.Load<ProjectContext>(ProjectContextLoadPath);

            if (contextPrefab == null)
            {
                Debug.LogWarning("The project context could not be created. Check that the Resources folder contains the ProjectContext prefab");
                return;
            }
            
            GameObject.Instantiate(contextPrefab);
        }
    }
}
