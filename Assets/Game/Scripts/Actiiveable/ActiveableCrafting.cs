using TimmyFramework;
using UnityEngine;

[RequireComponent(typeof(Activeable))]
public class ActiveableCrafting : MonoBehaviour, IActivable
{
    [SerializeField] private RecipeSetScheme _recipes;

    private UIWindowsController _uiWindowsController;

    private void Start()
    {
        if (Game.IsReady)
        {
            InitialSetup();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }

        GetComponent<Activeable>().SetActiveable(this);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void InitialSetup()
    {
        _uiWindowsController = Game.GetController<UIWindowsController>();
    }

    public void Activate()
    {
        _uiWindowsController.Crafter.SetRecipeSet(_recipes);
        _uiWindowsController.OpenWindow(_uiWindowsController.Crafter);
    }
}
