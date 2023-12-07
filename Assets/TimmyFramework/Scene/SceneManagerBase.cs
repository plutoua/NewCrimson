using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TimmyFramework;
using UnityEngine;

namespace TimmyFramework
{
    public abstract class SceneManagerBase
    {
        public event Action<Scene> OnSceneLoadedEvent;
        public Scene Scene { get; private set; }
        public bool IsLoading { get; private set; }

        protected Dictionary<string, SceneConfig> SceneConfigsMap;

        public SceneManagerBase()
        {
            SceneConfigsMap = new Dictionary<string, SceneConfig>();
            InitSceneMap();
        }

        public abstract void InitSceneMap();

        public Coroutine LoadCurrentSceneAsync()
        {
            if (IsLoading)
            {
                throw new Exception("Scene is loading now.");
            }

            var sceneName = SceneManager.GetActiveScene().name;
            var config = SceneConfigsMap[sceneName];

            return Coroutines.StartRoutine(LoadCurrentSceneRoutine(config));
        }
        
        private IEnumerator LoadCurrentSceneRoutine(SceneConfig sceneConfig)
        {
            IsLoading = true;
            yield return Coroutines.StartRoutine(InitializeSceneAsyncRoutine(sceneConfig));
            IsLoading = false;
            OnSceneLoadedEvent?.Invoke(Scene);
        }

        public Coroutine LoadNewSceneAsync(string sceneName)
        {
            if (IsLoading)
            {
                throw new Exception("Scene is loading now.");
            }

            var config = SceneConfigsMap[sceneName];

            return Coroutines.StartRoutine(LoadNewSceneRoutine(config));
        }

        private IEnumerator LoadNewSceneRoutine(SceneConfig sceneConfig)
        {
            IsLoading = true;
            yield return Coroutines.StartRoutine(LoadSceneAsyncRoutine(sceneConfig));
            yield return Coroutines.StartRoutine(InitializeSceneAsyncRoutine(sceneConfig));
            IsLoading = false;
            OnSceneLoadedEvent?.Invoke(Scene);
        }

    private IEnumerator LoadSceneAsyncRoutine(SceneConfig sceneConfig)
        {
            var async = SceneManager.LoadSceneAsync(sceneConfig.SceneName);
            async.allowSceneActivation = false;

            while (async.progress < .9f)
            {
                yield return null;
            }

            async.allowSceneActivation = true;
        }
        
        private IEnumerator InitializeSceneAsyncRoutine(SceneConfig sceneConfig)
        {
            Scene = new Scene(sceneConfig);
            yield return Scene.InitializeAsync();
        }

        public T GetStorage<T>()
            where T : IStorage
        {
            return Scene.GetStorage<T>();
        }

        public T GetController<T>()
            where T : IController
        {
            return Scene.GetController<T>();
        }
    }
}