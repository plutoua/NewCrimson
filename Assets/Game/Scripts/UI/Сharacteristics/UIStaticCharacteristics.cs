using TimmyFramework;
using UnityEngine;

public class UIStaticCharacteristics : MonoBehaviour, UIIWindow
{
    private MoveableWindow _moveableWindow;
    private UIWindowsController _windowsController;
    private void Start()
    {
        _moveableWindow = GetComponentInParent<MoveableWindow>();
        _moveableWindow.CloseButtonEvent += OnCloseButton;

        if (Game.IsReady)
        {
            SetupControllers();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetupControllers();
    }

    private void SetupControllers()
    {
        _windowsController = Game.GetController<UIWindowsController>();
        _windowsController.SetCharacteristics(this);
    }

    private void OnCloseButton()
    {
        _windowsController.CloseWindow(this);
    }

    public void Activate()
    {
        _moveableWindow.Activate();
    }

    public void Deactivate()
    {
        _moveableWindow.Deactivate();
    }
}
