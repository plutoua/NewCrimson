using System;
using TMPro;
using UnityEngine;

namespace TimmyFramework
{
    public class CameraMoverMobile : CameraMoverBase
    {
        private Touch _touch;
        
        private void Update()
        {
            if (Input.touchCount == 1)
            {
                _touch = Input.GetTouch(0);
                if (_touch.phase == TouchPhase.Moved)
                {
                    var direction = new Vector3(_touch.deltaPosition.x, _touch.deltaPosition.y).normalized;
                    MoveCamera(direction);
                }
            }
        }

        protected override Vector3 GetNextPosition(Vector3 direction)
        {
            return _camera.transform.position + direction * -1 * _cameraMoveSpeed * Time.deltaTime;
        }
    }
}