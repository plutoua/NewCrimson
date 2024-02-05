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

        SetTooltipPosition(tooltipSize, Input.mousePosition);

        _canvasGroup.alpha = 1.0f;
    }

    private void ShowTooltip(string text, Vector2 position)
    {
        _tooltip.text = text;

        Vector2 tooltipSize = new Vector2(_tooltip.preferredWidth + 12f, _tooltip.preferredHeight + 8f);
        _backgroundRectTransform.sizeDelta = tooltipSize;

        SetTooltipPosition(tooltipSize, position);

        _canvasGroup.alpha = 1.0f;
    }

    public static void Show(string text)
    {
        _instance.ShowTooltip(text);
    }

    public static void Show(string text, Vector2 position)
    {
        _instance.ShowTooltip(text, position);
    }

    private void HideTooltip()
    {
        _canvasGroup.alpha = 0;
    }

    public static void Hide()
    {
        _instance.HideTooltip();
    }

    private void ChangeTooltipPosition(Vector2 position)
    {
        var tooltipSize = _backgroundRectTransform.sizeDelta;
        SetTooltipPosition(tooltipSize, position);
    }

    public static void ChangePosition(Vector2 position)
    {
        _instance.ChangeTooltipPosition(position);
    }

    private void SetTooltipPosition(Vector2 sizes, Vector2 mousePosition)
    {
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
