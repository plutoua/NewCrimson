using System.Collections;
using UnityEngine;

namespace TimmyFramework
{
    public class Scene
    {
        private ControllersBase _controllersBase;
        private StoragesBase _storagesBase;
        private SceneConfig _sceneConfig;

        public Scene(SceneConfig sceneConfig)
        {
            _sceneConfig = sceneConfig;
            _controllersBase = new ControllersBase(_sceneConfig);
            _storagesBase = new StoragesBase(_sceneConfig);
        }

        public Coroutine InitializeAsync()
        {
            return Coroutines.StartRoutine(InitializeRoutine());
        }

        private IEnumerator InitializeRoutine()
        {
            _controllersBase.CreateAllControllers();
            _storagesBase.CreateAllStorages();
            yield return null;
            
            _controllersBase.SendOnCreateToAllControllers();
            _storagesBase.SendOnCreateToAllStorages();
            yield return null;
            
            _controllersBase.SendOnAwakeToAllIterators();
            _storagesBase.SendOnAwakeToAllStorages();
            yield return null;
            
            _controllersBase.InitializeAllControllers();
            _storagesBase.InitializeAllStorages();
            yield return null;
            
            _controllersBase.SendOnStartToAllIterators();
            _storagesBase.SendOnStartToAllStorages();
        }

        public T GetStorage<T>()
            where T : IStorage
        {
            return _storagesBase.GetStorage<T>();
        }

        public T GetController<T>()
            where T : IController
        {
            return _controllersBase.GetController<T>();
        }
    }
}