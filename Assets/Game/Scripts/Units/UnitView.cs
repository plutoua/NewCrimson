using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private ViewScheme _body;
    [SerializeField] private ViewScheme _head;

    private SpriteRenderer _bodyRenderer;
    private SpriteRenderer _headRenderer;

    private void Start()
    {
        _bodyRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
        _headRenderer = transform.Find("Head").GetComponent<SpriteRenderer>();

        var playerRotator = GetComponentInParent<PlayerRotator>();
        playerRotator.PlayerRotateEvent += OnPlayerRotate;
    }

    private void Update()
    {
        return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetBodySprite(_body.Profile, true);
            SetHeadSprite(_head.Profile, true);
        }
        else if(Input.GetKeyDown(KeyCode.D)) 
        {
            SetBodySprite(_body.Profile, false);
            SetHeadSprite(_head.Profile, false);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SetBodySprite(_body.Back, false);
            SetHeadSprite(_head.Back, false);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetBodySprite(_body.Front, false);
            SetHeadSprite(_head.Front, false);
        }
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
        if(angle <= 45 || angle >= 315)
        {
            SetBodySprite(_body.Front, false);
            SetHeadSprite(_head.Front, false);
        }
        else if(angle > 45 && angle < 135)
        {
            SetBodySprite(_body.Profile, false);
            SetHeadSprite(_head.Profile, false);
        }
        else if(angle >= 135 &&  angle <= 225)
        {
            SetBodySprite(_body.Back, false);
            SetHeadSprite(_head.Back, false);
        }
        else if(angle > 225 && angle < 315)
        {
            SetBodySprite(_body.Profile, true);
            SetHeadSprite(_head.Profile, true);
        }
    }
}
