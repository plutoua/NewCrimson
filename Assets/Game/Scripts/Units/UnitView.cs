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
    }

    private void Update()
    {
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
}
