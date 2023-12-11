using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    [SerializeField, Range(1f,20)] private float _minZoomValue;
    [SerializeField, Range(1f, 20)] private float _maxZoomValue;
    [SerializeField, Range(1f, 20)] private float _zoomSpeed;
    [SerializeField, Range(1f, 20)] private float _zoomStep;

    private Camera _camera;

    private float _targetZoom;
    private bool _isZooming;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _isZooming = false;
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomTo(Input.mouseScrollDelta.y);
        }

        if (_isZooming)
        {
            Zoom();
        }
        
    }

    private void Zoom()
    {
        var zoomDifference = _targetZoom - _camera.orthographicSize;

        _camera.orthographicSize += zoomDifference * _zoomSpeed * Time.deltaTime;
        TryNotZooming(zoomDifference);
    }

    private void TryNotZooming(float zoomDifference)
    {
        var orthographicSize = _camera.orthographicSize;

        if(orthographicSize > _targetZoom - 0.01f && orthographicSize < _targetZoom + 0.01f)
        {
            _camera.orthographicSize = _targetZoom;
            _isZooming = false;
            return;
        }

        if(zoomDifference > 0 && orthographicSize > _targetZoom ||
            zoomDifference < 0 && orthographicSize < _targetZoom)
        {
            _camera.orthographicSize = _targetZoom;
            _isZooming = false;
        }
    }

    private void ZoomTo(float zoomTarget)
    {
        _targetZoom = _camera.orthographicSize - _zoomStep * zoomTarget;

        if(_targetZoom < _minZoomValue)
        {
            _targetZoom = _minZoomValue;
        }
        if (_targetZoom > _maxZoomValue)
        {
            _targetZoom = _maxZoomValue;
        }

        _isZooming = true;
    }
}
