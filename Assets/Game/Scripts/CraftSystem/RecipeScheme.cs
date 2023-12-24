using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipeScheme", menuName = "ScriptableObjects/New RecipeScheme", order = 3)]
public class RecipeScheme : ScriptableObject
{
    [SerializeField] private ItemScheme _resultItem;
    [SerializeField] private int _resultAmount;
    [SerializeField] private ItemScheme[] _needetItems;
    [SerializeField] private int[] _needetAmount;

    public InventoryItem GetItem()
    {
        return new InventoryItem(_resultItem, _resultAmount);
    }

    public InventoryItem[] GetIngredients()
    {
        if(_needetItems.Length != _needetAmount.Length)
        {
            throw new Exception("Arrays is different size.");
        }
        InventoryItem[] ingredients = new InventoryItem[_needetItems.Length];

        for(int i = 0; i < _needetItems.Length; i++)
        {
            ingredients[i] = new InventoryItem(_needetItems[i], _needetAmount[i]);
        }

        return ingredients;
    }
}
