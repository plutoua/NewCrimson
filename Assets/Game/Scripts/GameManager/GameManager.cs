using TimmyFramework;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Game.Run<SceneManager_SampleScene>();
    }
}


