using TimmyFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemMover : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Transform _transformParent;
    private UIWindowsController _windowsController;
    private bool _isCanDrag;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();

        _isCanDrag = true;

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
        if (_isCanDrag)
        {
            _rectTransform.anchoredPosition += eventData.delta / _windowsController.Canvas.scaleFactor;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_isCanDrag )
        {
            _windowsController.SetItemOnMove(this);
            _transformParent = _rectTransform.parent;
            _rectTransform.SetParent(_windowsController.Moveable.transform);
            _canvasGroup.alpha = 0.6f;
            _canvasGroup.blocksRaycasts = false;
        }  
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isCanDrag)
        {
            MakeEndDrag();
        }
    }

    public void MakeEndDrag()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        if (_rectTransform.parent == _windowsController.Moveable.transform)
        {
            _rectTransform.SetParent(_transformParent);
            _rectTransform.localPosition = Vector3.zero;
        }
    }

    public void SetDragable(bool dragable)
    {
        _isCanDrag = dragable;
    }
}
