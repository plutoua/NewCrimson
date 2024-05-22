using TimmyFramework;
using UnityEngine;

public class UIWindowSystem : MonoBehaviour
{
    [SerializeField] private RecipeSetScheme _selfRecipes;

    private UIWindowsController _windowsController;
    private bool _isUIAvalible;

    private void Start()
    {
        _isUIAvalible = true;
        if (Game.IsReady)
        {
            WhenGameReady();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void Update()
    {
        if(_windowsController == null || !_isUIAvalible) 
        {
            return;
        }

        if(Input.GetKeyUp(KeyCode.I)) 
        {
             ActivateInventory();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            _windowsController.Crafter.SetRecipeSet(_selfRecipes);
            _windowsController.OpenWindow(_windowsController.Crafter);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            _windowsController.OpenWindow(_windowsController.Ñharacteristics);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            _windowsController.OpenWindow(_windowsController.Trade);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            _windowsController.OpenWindow(_windowsController.Equipment);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _windowsController.CloseAllWindows();
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        WhenGameReady();
    }

    private void WhenGameReady()
    {
        _windowsController = Game.GetController<UIWindowsController>();
        _windowsController.SetCanvas(GetComponent<Canvas>());
        var blocker = Game.GetController<BlockerController>();
        blocker.ConsoleChangeActivityEvent += OnConsoleAction;
    }

    private void ActivateInventory()
    {
        _windowsController.OpenInventory();
    }

    private void OnConsoleAction(bool enable)
    {
        _isUIAvalible = enable;
    }
}
