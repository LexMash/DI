﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaCon
{
    public class ProjectContext : MonoBehaviour
    {
        [SerializeField] private DIInstaller[] registrators;

        private DIContainer container;
        private SceneContext currentSceneContext;

        private void Awake()
        {
            container = new DIContainer();

            RegisterProjectContext(container);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

            DontDestroyOnLoad(this);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {            
            currentSceneContext = GetSceneContext(scene);
            currentSceneContext.InitContext(container);
        }

        private void RegisterProjectContext(DIContainer container)
        {
            var count = registrators.Length;
            for (int i = 0; i < count; i++)
            {
                DIInstaller registrator = registrators[i];

                registrator.InstallBindings(container);
            }

            container.BuildContext();
        }

        private SceneContext GetSceneContext(Scene scene)
        {
            var roots = scene.GetRootGameObjects();

            var count = roots.Length;
            for (int i = 0; i < count; i++)
            {
                GameObject root = roots[i];

                if (root.TryGetComponent(out SceneContext context))
                    return context;
            }

            throw new MissingComponentException($"Cannot find SceneContext on scene {scene.name}");
        }

        private void OnSceneUnloaded(Scene scene)
        {
            currentSceneContext = null;
        }

        private void OnDestroy()
        {
            container.Dispose();
            container = null;
            currentSceneContext = null;

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}
