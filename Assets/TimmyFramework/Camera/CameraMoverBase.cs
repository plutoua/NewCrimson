using UnityEngine;

namespace TimmyFramework
{
    public abstract class CameraMoverBase : MonoBehaviour
    {
        [SerializeField] protected Camera _camera;
        [SerializeField] protected float _cameraMoveSpeed;

        private Vector3 _maxPoint;
        private Vector3 _minPoint;

        private void Start()
        {
            var cameraRealHalfSize = Vector3Abs(
                _camera.transform.position - _camera.ScreenToWorldPoint(Vector3.zero)
            );

            _maxPoint = transform.position + transform.localScale / 2f - cameraRealHalfSize;
            _minPoint = transform.position - transform.localScale / 2f + cameraRealHalfSize;
        }
        
        protected void MoveCamera(Vector3 direction)
        {
            var newCameraPosition = GetNextPosition(direction);

            if (newCameraPosition.x > _maxPoint.x) newCameraPosition.x = _maxPoint.x;
            if (newCameraPosition.x < _minPoint.x) newCameraPosition.x = _minPoint.x;
            if (newCameraPosition.y > _maxPoint.y) newCameraPosition.y = _maxPoint.y;
            if (newCameraPosition.y < _minPoint.y) newCameraPosition.y = _minPoint.y;

            _camera.transform.position = newCameraPosition;
        }

        private Vector3 Vector3Abs(Vector3 vector3)
        {
            vector3.x = Mathf.Abs(vector3.x);
            vector3.y = Mathf.Abs(vector3.y);
            vector3.z = Mathf.Abs(vector3.z);
            
            return vector3;
        }

        protected abstract Vector3 GetNextPosition(Vector3 direction);
    }
}