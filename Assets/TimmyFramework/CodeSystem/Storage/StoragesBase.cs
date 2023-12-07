using System;
using System.Collections.Generic;
using System.Linq;

namespace TimmyFramework
{
    public class StoragesBase
    {
        private Dictionary<Type, IStorage> _storagesMap;
        private SceneConfig _sceneConfig;

        public StoragesBase(SceneConfig sceneConfig)
        {
            _sceneConfig = sceneConfig;
        }

        public void CreateAllStorages()
        {
            _storagesMap = _sceneConfig.CreateAllStorages();
        }

        public void SendOnCreateToAllStorages()
        {
            var storages = _storagesMap.Values.Where(x=> x is IOnCreate).ToArray();

            for (var i = 0; i < storages.Length; i++)
            {
                ((IOnCreate) storages[i]).OnCreate();
            }
        }
        
        public void InitializeAllStorages()
        {
            var allStorages = _storagesMap.Values;

            foreach (var storage in allStorages)
            {
                storage.Initialize();
            }
        }
        
        public void SendOnStartToAllStorages()
        {
            var storages = _storagesMap.Values.Where(x=> x is IOnStart).ToArray();

            for (var i = 0; i < storages.Length; i++)
            {
                ((IOnStart) storages[i]).OnStart();
            }
        }
        
        public void SendOnAwakeToAllStorages()
        {
            var storages = _storagesMap.Values.Where(x=> x is IOnAwake).ToArray();

            for (var i = 0; i < storages.Length; i++)
            {
                ((IOnAwake) storages[i]).OnAwake();
            }
        }
        
        public T GetStorage<T>()
            where T: IStorage
        {
            var type = typeof(T);
            return (T)_storagesMap[type];
        }
    }
}