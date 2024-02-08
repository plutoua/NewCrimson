using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITooltipIItem : MonoBehaviour
{
    private TMP_Text _itemName;
    private Image _itemIcon;
    private TMP_Text _itemPrice;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Camera _camera;
    private CanvasScaler _canvasScaler;

    private static UITooltipIItem _instance;

    private void Start()
    {
        _instance = this;

        _itemName = transform.Find("ItemName").GetComponent<TMP_Text>();
        _itemIcon = transform.Find("ItemIcon").GetComponent<Image>();
        _itemPrice = transform.Find("ItemPrice").GetComponent<TMP_Text>();

        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
        _canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    private void ShowTooltip(InventoryItem inventoryItem)
    {
        _canvasGroup.alpha = 1.0f;
        _itemName.text = inventoryItem.Name;
        _itemIcon.sprite = inventoryItem.ItemScheme.Sprite;
        _itemPrice.text = "Price: " + inventoryItem.Price;

        CalculatePosition();
    }

    public static void Show(InventoryItem inventoryItem)
    {
        _instance.ShowTooltip(inventoryItem);
    }

    private void HideTooltip()
    {
        _canvasGroup.alpha = 0;
    }

    public static void Hide()
    {
        _instance.HideTooltip();
    }

    private void CalculatePosition()
    {
        var mousePosition = Input.mousePosition;

        var coefficientWidth = _canvasScaler.referenceResolution.x / _camera.pixelWidth;
        var coefficientHeight = _canvasScaler.referenceResolution.y / _camera.pixelHeight;
        mousePosition.x *= coefficientWidth;
        mousePosition.y *= coefficientHeight;

        var windowSizes = _rectTransform.sizeDelta;
        windowSizes.x *= coefficientWidth;
        windowSizes.y *= coefficientHeight;

        Vector2 finalPosition = Vector2.zero;

        if (mousePosition.x + windowSizes.x > _camera.pixelWidth)
        {
            finalPosition.x = _camera.pixelWidth - windowSizes.x;
        }
        else
        {
            finalPosition.x = mousePosition.x;
        }

        if (mousePosition.y + windowSizes.y > _camera.pixelHeight)
        {
            finalPosition.y = _camera.pixelHeight - windowSizes.y;
        }
        else
        {
            finalPosition.y = mousePosition.y;
        }

        _rectTransform.anchoredPosition = finalPosition;
    }
}
