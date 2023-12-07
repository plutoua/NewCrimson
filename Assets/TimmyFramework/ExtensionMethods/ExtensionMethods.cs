using System;
using System.Collections.Generic;

namespace TimmyFramework
{
    public static class ExtensionMethods
    {
        public static void CreateAndAdd<T>(this Dictionary<Type, IController> controllerMap)
        where T : IController, new()
        {
            var type = typeof(T);
            var controller = new T();
            controllerMap[type] = controller;
        }
        
        public static void CreateAndAdd<T>(this Dictionary<Type, IStorage> storageMap)
            where T : IStorage, new()
        {
            var type = typeof(T);
            var storage = new T();
            storageMap[type] = storage;
        }
    }
}