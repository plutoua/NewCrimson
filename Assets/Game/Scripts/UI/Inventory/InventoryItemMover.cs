using TimmyFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemMover : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Transform _transformParent;
    private UIWindowsController _windowsController;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        if (Game.IsReady)
        {
            SetupWindowController();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }
    private void SetupWindowController()
    {
        _windowsController = Game.GetController<UIWindowsController>();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetupWindowController();
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
        _transformParent = _rectTransform.parent;
        _rectTransform.SetParent(_windowsController.Moveable.transform);
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        if(_rectTransform.parent == _windowsController.Moveable.transform)
        {
            _rectTransform.SetParent(_transformParent);
            _rectTransform.localPosition = Vector3.zero;
        }
    }
}
