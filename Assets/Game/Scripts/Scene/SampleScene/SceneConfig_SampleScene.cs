using System;
using System.Collections.Generic;
using TimmyFramework;


public class SceneConfig_SampleScene : SceneConfig
{
    public const string SCENE_NAME = "SampleScene";
    public override string SceneName => SCENE_NAME;

    public override Dictionary<Type, IController> CreateAllControllers()
    {
        var controllerMap = new Dictionary<Type, IController>();
        controllerMap.CreateAndAdd<MouseLocatorController>();
        controllerMap.CreateAndAdd<PlayerLocatorController>();
        controllerMap.CreateAndAdd<EnemysLocatorController>();
        controllerMap.CreateAndAdd<PlayerStatController>();
        controllerMap.CreateAndAdd<InventoryController>();
        controllerMap.CreateAndAdd<GroundDetectionController>();
        controllerMap.CreateAndAdd<UIWindowsController>();
        controllerMap.CreateAndAdd<ConsoleController>();
        return controllerMap;
    }

    public override Dictionary<Type, IStorage> CreateAllStorages()
    {
        var storageMap = new Dictionary<Type, IStorage>();
        storageMap.CreateAndAdd<ItemStorage>();
        storageMap.CreateAndAdd<LanguageStorage>();
        return storageMap;
    }
}
