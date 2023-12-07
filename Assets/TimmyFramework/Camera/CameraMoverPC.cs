using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace TimmyFramework
{
    public class CameraMoverPC : CameraMoverBase
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MoveCamera(Vector3.left);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MoveCamera(Vector3.right);
            }
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                MoveCamera(Vector3.up);
            }
            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                MoveCamera(Vector3.down);
            }
        }

        protected override Vector3 GetNextPosition(Vector3 direction)
        {
            return _camera.transform.position + direction * _cameraMoveSpeed * Time.deltaTime;
        }
    }
}

