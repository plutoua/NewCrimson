using TimmyFramework;
using UnityEngine;

public class UITrade : MonoBehaviour, UIIWindow
{
    [SerializeField] private UISellInventory _uiSellInventory;

    private MoveableWindow _moveableWindow;
    private UIWindowsController _windowsController;

    private void Start()
    {
        _moveableWindow = GetComponentInParent<MoveableWindow>();
        _moveableWindow.CloseButtonEvent += OnCloseButton;

        if (Game.IsReady)
        {
            InitialSetup();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void InitialSetup()
    {
        _windowsController = Game.GetController<UIWindowsController>();
        _windowsController.SetTrade(this);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
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
        _uiSellInventory.ReturnItems();
        _moveableWindow.Deactivate();
    }
}
