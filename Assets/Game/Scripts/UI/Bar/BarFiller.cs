using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarFiller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float _width;
    private RectTransform _fallBar;
    private RectTransform _activeBar;
    private TMP_Text _barValue;
    private TMP_Text _barTooltipValue;

    private float _fullHP;
    private float _nowHP;
    private float _fallHP;
    private bool _isFall;

    private void Start()
    {
        _width = GetComponent<RectTransform>().rect.width;
        _fallBar = transform.Find("BarFall").GetComponent<RectTransform>();
        _activeBar = transform.Find("BarActive").GetComponent<RectTransform>();
        _barValue = transform.Find("BarValue").GetComponent<TMP_Text>();
        _barTooltipValue = transform.Find("BarTooltip").GetComponent<TMP_Text>();
        _barTooltipValue.gameObject.SetActive(false);

        SetMaxHP(100);
        _isFall = false;
    }

    public void SetMaxHP(float maxHP)
    {
        _fullHP = maxHP;
        _nowHP = _fullHP;
        _barValue.text = _nowHP.ToString();
        _barTooltipValue.text = _nowHP + " / " + _fullHP;
    }

    public void SetNowHP(float nowHP)
    {
        CalculateBarValue(nowHP);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CalculateBarValue(_nowHP - 10);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            CalculateBarValue(_nowHP + 10);
        }

        if (_isFall)
        {
            CalculateFallBar();
        }
    }

    private void CalculateBarValue(float value)
    {
        _fallHP = _nowHP;
        _nowHP = value;
        var sizes = _activeBar.sizeDelta;
        sizes.x = _width * _nowHP / _fullHP;
        _activeBar.sizeDelta = sizes;

        _isFall = true;
        _barValue.text = _nowHP.ToString();
        _barTooltipValue.text = _nowHP + " / " + _fullHP;
    }

    private void CalculateFallBar()
    {
        _fallHP = Mathf.Lerp(_fallHP, _nowHP, 5 * Time.deltaTime);
        

        if((_nowHP - 0.2f) < _fallHP && (_nowHP + 0.2f) > _fallHP)
        {
            _isFall =false;
            _fallHP = _nowHP;
        }

        var sizes = _fallBar.sizeDelta;
        sizes.x = _width * _fallHP / _fullHP;
        _fallBar.sizeDelta = sizes;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _barTooltipValue.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _barTooltipValue.gameObject.SetActive(false);
    }
}
