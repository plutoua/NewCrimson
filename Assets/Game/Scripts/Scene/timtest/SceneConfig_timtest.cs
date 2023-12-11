using System;
using System.Collections.Generic;
using TimmyFramework;


public class SceneConfig_timtest : SceneConfig
{
    public const string SCENE_NAME = "timtest";
    public override string SceneName => SCENE_NAME;

    public override Dictionary<Type, IController> CreateAllControllers()
    {
        var controllerMap = new Dictionary<Type, IController>();
        controllerMap.CreateAndAdd<MouseLocatorController>();
        controllerMap.CreateAndAdd<PlayerLocatorController>();
        return controllerMap;
    }

    public override Dictionary<Type, IStorage> CreateAllStorages()
    {
        var storageMap = new Dictionary<Type, IStorage>();
        
        return storageMap;
    }
}
