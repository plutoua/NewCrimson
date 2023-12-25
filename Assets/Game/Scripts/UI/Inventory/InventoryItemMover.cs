using TimmyFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemMover : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Transform _transformParent;
    private Transform _moveableParent;
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
        _moveableParent = _windowsController.Moveable.transform;
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
        _windowsController.SetItemOnMove(this);
        _transformParent = _rectTransform.parent;
        _rectTransform.SetParent(_moveableParent);
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MakeEndDrag();
    }

    public void MakeEndDrag()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        if (_rectTransform.parent == _moveableParent)
        {
            _rectTransform.SetParent(_transformParent);
            _rectTransform.localPosition = Vector3.zero;
        }
    }
}
