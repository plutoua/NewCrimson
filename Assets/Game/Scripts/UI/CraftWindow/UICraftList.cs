using TimmyFramework;
using UnityEngine;

public class UICraftList : MonoBehaviour
{
    // test
    [SerializeField] private RecipeSetScheme _alowedRecipes;
    [SerializeField] private RecipeSetScheme _knowedRecipes;

    [SerializeField] private UICraftListItem _itemPrefab;

    private UICraftListItem _activeItem;
    private UICrafter _crafter;

    private void Start()
    {
        _crafter = GetComponentInParent<UICrafter>();

        if (Game.IsReady)
        {
            MakeCraftList();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    public void SetActiveItem(UICraftListItem item)
    {
        if(_activeItem != null)
        {
            _activeItem.MakeNonActive();
        }

        _activeItem = item;
        _crafter.SetActiveRecipe(item.Recipe);
    }

    private void MakeCraftList()
    {
        UICraftListItem tempItem;
        foreach (var recipe in _alowedRecipes.Recepts)
        {
            tempItem = Instantiate(_itemPrefab, transform);
            tempItem.SetController(this);
            tempItem.SetRecipe(recipe);
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        MakeCraftList();
    }
}
