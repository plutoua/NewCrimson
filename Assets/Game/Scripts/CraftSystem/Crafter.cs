public class Crafter
{
    private RecipeScheme _recipe;
    private Inventory _inventory;

    private bool _isReadyToCraft;

    public Crafter(Inventory inventory)
    {
        _inventory = inventory;
        _inventory.InventoryUpdatedEvent += CheckIngredients;
        _isReadyToCraft = false;
    }

    public void SetRecipe(RecipeScheme recipe)
    {
        _recipe = recipe;
        CheckIngredients();
    }

    public void Craft()
    {
        if(_isReadyToCraft)
        {
            _inventory.InventoryUpdatedEvent -= CheckIngredients;
            var ingredients = _recipe.GetIngredients();
            foreach (var ingredient in ingredients)
            {
                _inventory.Remove(ingredient);
            }
            _inventory.InventoryUpdatedEvent += CheckIngredients;
            _inventory.Add(_recipe.GetItem());     
        }
    }

    private void CheckIngredients()
    {
        if (_recipe == null) 
        {
            return;
        }

        _isReadyToCraft = true;

        var ingredients = _recipe.GetIngredients();
        InventoryItem tempItem;
        foreach (var ingredient in ingredients)
        {
            tempItem = _inventory.GetAllOfID(ingredient);
            if (tempItem.Amount < ingredient.Amount)
            {
                _isReadyToCraft = false;
            }
        }
    }
}
