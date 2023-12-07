using System;
using System.Collections;

namespace TimmyFramework
{
    public static class Game
    {
        public static event Action OnInitializedEvent;
        public static SceneManagerBase SceneManager { get; private set; }
        public static bool IsReady { get; private set; }

        public static void Run<T>()
        where T : SceneManagerBase, new()
        {
            IsReady = false;
            SceneManager = new T();
            Coroutines.StartRoutine(InitializeGameRoutine());
        }

        private static IEnumerator InitializeGameRoutine()
        {
            SceneManager.InitSceneMap();
            yield return SceneManager.LoadCurrentSceneAsync();
            IsReady = true;
            OnInitializedEvent?.Invoke();
        }
        
        public static T GetStorage<T>()
            where T : IStorage
        {
            return SceneManager.GetStorage<T>();
        }

        public static T GetController<T>()
            where T : IController
        {
            return SceneManager.GetController<T>();
        }
    }
}