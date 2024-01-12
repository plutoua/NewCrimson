using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class MoveableWindow : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private TMP_Text _windowName;
    [SerializeField] private Button _closeButton;

    public event UnityAction CloseButtonEvent; 

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        _closeButton.onClick.AddListener(CloseButton);
    }

    public void Activate()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Deactivate()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    private void CloseButton()
    {
        CloseButtonEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
    }

    public void SetSizes(float width, float height)
    {
        _rectTransform.sizeDelta = new Vector3(width, height + 70);
    }

    public void SetPosition(float width, float height)
    {
        _rectTransform.anchoredPosition = new Vector3(width, height);
    }

    public void SetName(string name)
    {
        _windowName.text = name;
    }
}
