using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIngredientListItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _amount;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _problemColor;

    private InventoryItem _item;

    public void SetIngredient(InventoryItem item)
    {
        _item = item;

        _icon.sprite = _item.Sprite;
        _name.text = _item.Name;
        _amount.text = _item.Amount.ToString();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        _item = null;
    }

    public void CheckAvaliable(Inventory inventory)
    {
        if(_item == null)
        {
            return;
        }

        var itemInInventory = inventory.GetAllOfID(_item);

        if(itemInInventory.Amount >= _item.Amount)
        {
            _name.color = _defaultColor;
            _amount.color = _defaultColor;
        }
        else
        {
            _name.color = _problemColor;
            _amount.color = _problemColor;
        }
    }
}
