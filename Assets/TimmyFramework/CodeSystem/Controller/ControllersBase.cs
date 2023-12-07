using System;
using System.Collections.Generic;
using System.Linq;

namespace TimmyFramework
{
    public class ControllersBase
    {
        private Dictionary<Type, IController> _controllersMap;
        private readonly SceneConfig _sceneConfig;

        public ControllersBase(SceneConfig sceneConfig)
        {
            _sceneConfig = sceneConfig;
        }

        public void CreateAllControllers()
        {
            _controllersMap = _sceneConfig.CreateAllControllers();
        }

        public void SendOnCreateToAllControllers()
        {
            var controllers = _controllersMap.Values.Where(x=> x is IOnCreate).ToArray();

            for (var i = 0; i < controllers.Length; i++)
            {
                ((IOnCreate) controllers[i]).OnCreate();
            }
        }
        
        public void InitializeAllControllers()
        {
            var controllers = _controllersMap.Values;

            foreach (var controller in controllers)
            {
                controller.Initialize();
            }
        }
        
        public void SendOnStartToAllIterators()
        {
            var controllers = _controllersMap.Values.Where(x=> x is IOnStart).ToArray();

            for (var i = 0; i < controllers.Length; i++)
            {
                ((IOnStart) controllers[i]).OnStart();
            }
        }
        
        public void SendOnAwakeToAllIterators()
        {
            var controllers = _controllersMap.Values.Where(x=> x is IOnAwake).ToArray();

            for (var i = 0; i < controllers.Length; i++)
            {
                ((IOnAwake) controllers[i]).OnAwake();
            }
        }
        
        public T GetController<T>()
        where T: IController
        {
            var type = typeof(T);
            return (T)_controllersMap[type];
        }
    }
}