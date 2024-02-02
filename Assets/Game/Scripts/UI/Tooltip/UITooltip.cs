using TMPro;
using UnityEngine;

public class UITooltip : MonoBehaviour
{
    private TMP_Text _tooltip;
    private RectTransform _backgroundRectTransform;
    private RectTransform _selfRectTransform;
    private CanvasGroup _canvasGroup;
    private Camera _camera;

    private static UITooltip _instance;

    private void Awake()
    {
        _instance = this;

        _selfRectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _camera = Camera.main;
        _backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        _tooltip = GetComponentInChildren<TMP_Text>();
    }

    private void ShowTooltip(string text)
    {
        _tooltip.text = text;

        Vector2 tooltipSize = new Vector2(_tooltip.preferredWidth + 12f, _tooltip.preferredHeight + 8f);
        _backgroundRectTransform.sizeDelta = tooltipSize;

        SetTooltipPosition(tooltipSize);

        _canvasGroup.alpha = 1.0f;
    }

    public static void Show(string text)
    {
        _instance.ShowTooltip(text);
    }

    private void HideTooltip()
    {
        _canvasGroup.alpha = 0;
    }

    public static void Hide()
    {
        _instance.HideTooltip();
    }

    private void SetTooltipPosition(Vector2 sizes)
    {
        var mousePosition = Input.mousePosition;
        Vector2 finalPosition = Vector2.zero;

        if(mousePosition.x + sizes.x > _camera.pixelWidth)
        {
            finalPosition.x = _camera.pixelWidth - sizes.x;
        }
        else
        {
            finalPosition.x = mousePosition.x;
        }

        if(mousePosition.y + sizes.y > _camera.pixelHeight)
        {
            finalPosition.y = _camera.pixelHeight - sizes.y;
        }
        else
        {
            finalPosition.y = mousePosition.y;
        }

        _selfRectTransform.anchoredPosition = finalPosition;
    }
}
