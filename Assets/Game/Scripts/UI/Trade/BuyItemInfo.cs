using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemInfo : MonoBehaviour
{
    [Header("Main parts")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemPrice;
    [SerializeField] private Button _buyButton;

    [Header("Addition parts")]
    [SerializeField] private ItemScheme _coinScheme;

    [Header("Text colors")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _errorColor;

    private InventoryItem _activeItem;
    private Inventory _playerInventory;
    private Inventory _buyInventory;
    private InventoryItem _tempCoin;
    private bool _isCanBuy;

    private void Start()
    {
        _buyButton.onClick.AddListener(OnBuyButtonClick);
        _tempCoin = new InventoryItem(_coinScheme, 1);
        _isCanBuy = true;
    }
    public void SetItem(InventoryItem item)
    {
        _activeItem = item;
        SetInfo();
        CheckCoins();
    }

    public void SetInventory(Inventory playerInventory, Inventory buyInventory) 
    {
        _playerInventory = playerInventory;
        _playerInventory.InventoryUpdatedEvent += OnInventoryUpdate;

        _buyInventory = buyInventory;
    }

    private void SetInfo()
    {
        _itemIcon.enabled = true;
        _itemIcon.sprite = _activeItem.ItemScheme.Sprite;

        _itemName.text = _activeItem.Name;

        _itemPrice.text = _activeItem.Price.ToString();
    }

    private void CheckCoins() 
    {
        if (_activeItem == null)
        {
            return;    
        }

        var coinsInInventory = _playerInventory.GetAllOfID(_tempCoin);

        if (coinsInInventory.Amount >= _activeItem.Price)
        {
            _isCanBuy = true;
            _itemPrice.color = _defaultColor;
        }
        else
        {
            _isCanBuy = false;
            _itemPrice.color = _errorColor;
        }
    }

    private void OnBuyButtonClick()
    {
        if(_isCanBuy)
        {
            var tempItem = _activeItem.CloneItemWithAmount(1);
            var coinPrice = _tempCoin.CloneItemWithAmount(tempItem.Price);
            _buyInventory.Remove(tempItem);
            _playerInventory.Remove(coinPrice);
            _playerInventory.Add(tempItem);
        }
    }

    private void OnInventoryUpdate()
    {
        CheckCoins();
    }
}
