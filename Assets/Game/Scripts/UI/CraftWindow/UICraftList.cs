using System.Collections.Generic;
using UnityEngine;

public class UICraftList : MonoBehaviour
{
    [SerializeField] private UICraftListItem _itemPrefab;

    private UICraftListItem _activeItem;
    private UICrafter _crafter;
    private RecipeSetScheme _alowedRecipes;

    private List<UICraftListItem> _items;

    private void Start()
    {
        _crafter = GetComponentInParent<UICrafter>();
        _crafter.RecipeSetChangeEvent += OnRecipeSetChange;
        _items = new List<UICraftListItem>();
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

    private void DeactivateItem()
    {
        if (_activeItem != null)
        {
            _activeItem.MakeNonActive();
            _activeItem = null;
        }
    }

    private void OnRecipeSetChange()
    {
        _alowedRecipes = _crafter.RecipeSetScheme;
        DeactivateAll();
        MakeCraftList();
        ActivateCraftList();
        DeactivateItem();
    }

    private void MakeCraftList()
    {
        UICraftListItem tempItem;
        if(_alowedRecipes.Recepts.Count > _items.Count)
        {
            for(int i = _items.Count; i < _alowedRecipes.Recepts.Count; i++)
            {
                tempItem = Instantiate(_itemPrefab, transform);
                tempItem.SetController(this);
                tempItem.gameObject.SetActive(false);
                _items.Add(tempItem);
            }
        }
    }

    private void ActivateCraftList()
    {
        for(int i = 0; i < _alowedRecipes.Recepts.Count; i++)
        {
            _items[i].gameObject.SetActive(true);
            _items[i].SetRecipe(_alowedRecipes.Recepts[i]);
        }
    }

    private void DeactivateAll()
    {
        foreach(var item in _items)
        {
            item.gameObject.SetActive(false);
        }  
    }
}
