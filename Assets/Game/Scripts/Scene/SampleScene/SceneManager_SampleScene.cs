using UnityEngine.SceneManagement;
using TimmyFramework;


public sealed class SceneManager_SampleScene : SceneManagerBase
{
    public override void InitSceneMap()
    {
        SceneConfigsMap[SceneConfig_SampleScene.SCENE_NAME] = new SceneConfig_SampleScene();
    }
}
