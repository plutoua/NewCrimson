using UnityEngine;

public class Activeable : MonoBehaviour
{
    private bool _active;

    private void Start()
    {
        _active = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_active)
        {
            return;
        }

        if(collision.TryGetComponent(out ActivationZone activationZone)) 
        {
            _active = true;
            Debug.Log("On active zone - " + transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_active)
        {
            return;
        }

        if (collision.TryGetComponent(out ActivationZone activationZone))
        {
            _active = false;
            Debug.Log("Not on active zone - " + transform.position);
        }
    }
}
