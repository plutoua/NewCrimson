using TimmyFramework;
using UnityEngine;

public class UIWindowSystem : MonoBehaviour
{
    [SerializeField] private RecipeSetScheme _selfRecipes;

    private UIWindowsController _windowsController;

    private void Start()
    {
        if (Game.IsReady)
        {
            _windowsController = Game.GetController<UIWindowsController>();
            _windowsController.SetCanvas(GetComponent<Canvas>());
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

        if (Input.GetKeyUp(KeyCode.C))
        {
            _windowsController.Crafter.SetRecipeSet(_selfRecipes);
            _windowsController.OpenWindow(_windowsController.Crafter);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            _windowsController.OpenWindow(_windowsController.—haracteristics);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            _windowsController.OpenWindow(_windowsController.Trade);
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
        _windowsController.SetCanvas(GetComponent<Canvas>());
    }

    private void ActivateInventory()
    {
        _windowsController.OpenInventory();
    }
}
