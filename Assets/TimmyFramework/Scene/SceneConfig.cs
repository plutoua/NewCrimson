using System;
using System.Collections.Generic;

namespace TimmyFramework
{
    public abstract class SceneConfig
    {
        public abstract string SceneName { get; }
        public abstract Dictionary<Type, IController> CreateAllControllers();
        public abstract Dictionary<Type, IStorage> CreateAllStorages();
    }
}