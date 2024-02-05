using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Activeable : MonoBehaviour
{

    private bool _active;
    private Camera _camera;
    private IActivable _activable;

    private void Start()
    {
        _active = false;
        _camera = Camera.main;
    }

    private void Update()
    {
        if(_active)
        {
            UITooltip.ChangePosition(GetTooltipposition());
        }
        if(_active && Input.GetKey(KeyCode.F) && _activable != null) 
        {
            UITooltip.Hide();
            _activable.Activate();
        }
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
            UITooltip.Show("Press F to activate.", GetTooltipposition());
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
            UITooltip.Hide();
        }
    }

    private Vector2 GetTooltipposition()
    {
        return _camera.WorldToScreenPoint(transform.position);
    }

    public void SetActiveable(IActivable activable)
    {
        _activable = activable;
    }
}
