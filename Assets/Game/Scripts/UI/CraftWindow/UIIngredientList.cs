using UnityEngine;

public class UIIngredientList : MonoBehaviour
{
    [SerializeField] private UIIngredientListItem _itemPrefab;
    [SerializeField] private int _itemCount;

    private RecipeScheme _recipeScheme;
    private UIIngredientListItem[] _items;

    private void Start()
    {
        _items = new UIIngredientListItem[_itemCount];

        for (int i = 0; i < _itemCount; i++)
        {
            _items[i] = Instantiate(_itemPrefab, transform);
            _items[i].Deactivate();
        }
    }

    public void SetRecipe(RecipeScheme recipeScheme)
    {
        _recipeScheme = recipeScheme;
        DeactivateItems();
        SetIngredients();
    }

    public void CheckIngredients(Inventory inventory)
    {
        foreach (var item in _items)
        {
            item.CheckAvaliable(inventory);
        }
    }

    private void SetIngredients()
    {
        var ingredients = _recipeScheme.GetIngredients();
        for (int i = 0; i < ingredients.Length; i++)
        {
            _items[i].SetIngredient(ingredients[i]);
        }
    }

    private void DeactivateItems()
    {
        foreach (var item in _items)
        {
            item.Deactivate();
        }
    }
}
