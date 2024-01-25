using TimmyFramework;
using UnityEngine;

public class UIWindowSystem : MonoBehaviour
{
    // for test
    [SerializeField] private ItemScheme _forTest;

    private bool _isUIMode => _windowsController.IsUIMode;

    private UIWindowsController _windowsController;

    private void Start()
    {
        if (Game.IsReady)
        {
            _windowsController = Game.GetController<UIWindowsController>();
            // for test
            _windowsController.SetTest(_forTest);
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void Update()
    {
        if(_windowsController == null) 
        {
            return;
        }

        if(Input.GetKeyUp(KeyCode.I)) 
        {
             ActivateInventory();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            _windowsController.OpenInnerInventory();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            _windowsController.OpenWindow(_windowsController.Crafter);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            _windowsController.OpenWindow(_windowsController.Ñharacteristics);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _windowsController.CloseAllWindows();
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        _windowsController = Game.GetController<UIWindowsController>();
        // for test
        _windowsController.SetTest(_forTest);
    }

    private void ActivateInventory()
    {
        _windowsController.OpenInventory();
    }
}
