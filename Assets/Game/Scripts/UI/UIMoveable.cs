using TimmyFramework;
using UnityEngine;

public class UIMoveable : MonoBehaviour
{
    private UIWindowsController _windowsController;
    private void Start()
    {
        if (Game.IsReady)
        {
            SetupWindowController();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void SetupWindowController()
    {
        _windowsController = Game.GetController<UIWindowsController>();
        _windowsController.SetMoveable(this);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetupWindowController();
    }
}
