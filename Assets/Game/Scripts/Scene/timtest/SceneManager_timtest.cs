using UnityEngine.SceneManagement;
using TimmyFramework;


public sealed class SceneManager_timtest : SceneManagerBase
{
    public override void InitSceneMap()
    {
        SceneConfigsMap[SceneConfig_timtest.SCENE_NAME] = new SceneConfig_timtest();
    }
}
