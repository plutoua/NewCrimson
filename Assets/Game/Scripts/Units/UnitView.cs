using Unity.VisualScripting;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private ViewScheme _body;
    [SerializeField] private ViewScheme _head;

    private SpriteRenderer _bodyRenderer;
    private SpriteRenderer _headRenderer;

    private View _viewNow;

    public void SetEquipment(ViewScheme head, ViewScheme body)
    {
        if (head != null) _head = head;
        if (body != null) _body = body;

        UpdateView();
    }

    private void Start()
    {
        _bodyRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
        _headRenderer = transform.Find("Head").GetComponent<SpriteRenderer>();

        _viewNow = View.Front;
        UpdateView();

        var playerRotator = GetComponentInParent<PlayerRotator>();
        playerRotator.PlayerRotateEvent += OnPlayerRotate;
    }

    private void SetBodySprite(Sprite sprite, bool flip)
    {
        _bodyRenderer.sprite = sprite;
        _bodyRenderer.flipX = flip;
    }

    private void SetHeadSprite(Sprite sprite, bool flip)
    {
        _headRenderer.sprite = sprite;
        _headRenderer.flipX = flip;
    }

    private void OnPlayerRotate(float angle)
    {
        
        if(angle >= -45 && angle <= 45 || angle >= 315)
        {
            _viewNow = View.Front;
        }
        else if(angle > 45 && angle < 135)
        {
            _viewNow = View.Right;
        }
        else if(angle >= 135 &&  angle <= 225)
        {
            _viewNow = View.Back;
        }
        else if(angle > 225 && angle < 315 || angle < -45 && angle >= -90)
        {
            _viewNow = View.Left;
        }

        UpdateView();
    }

    private void UpdateView()
    {
        switch (_viewNow)
        {
            case View.Front:
                SetBodySprite(_body.Front, false);
                SetHeadSprite(_head.Front, false);
                break;
            case View.Right:
                SetBodySprite(_body.Profile, false);
                SetHeadSprite(_head.Profile, false);
                break;
            case View.Back:
                SetBodySprite(_body.Back, false);
                SetHeadSprite(_head.Back, false);
                break;
            case View.Left:
                SetBodySprite(_body.Profile, true);
                SetHeadSprite(_head.Profile, true);
                break;
        }
    }

    private enum View
    {
        Left,
        Right,
        Front,
        Back
    }
}
