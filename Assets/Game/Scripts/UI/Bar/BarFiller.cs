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

    private float _fullValue;
    private float _nowValue;
    private float _fallValue;
    private bool _isFall;

    private void Start()
    {
        _width = GetComponent<RectTransform>().rect.width;
        _fallBar = transform.Find("BarFall").GetComponent<RectTransform>();
        _activeBar = transform.Find("BarActive").GetComponent<RectTransform>();
        _barValue = transform.Find("BarValue").GetComponent<TMP_Text>();
        _barTooltipValue = transform.Find("BarTooltip").GetComponent<TMP_Text>();
        _barTooltipValue.gameObject.SetActive(false);

        _isFall = false;

        _fullValue = 100;
        _nowValue = 0;
    }

    public void SetMaxValue(float maxHP)
    {
        _fullValue = maxHP;

        if (_nowValue == 0) _nowValue = _fullValue;

        CalculateBarValue(_nowValue);
    }

    public void SetNowValue(float nowHP)
    {
        CalculateBarValue(nowHP);
    }

    private void Update()
    {
        if (_isFall)
        {
            CalculateFallBar();
        }
    }

    private void CalculateBarValue(float value)
    {
        _fallValue = _nowValue;
        _nowValue = value;
        var sizes = _activeBar.sizeDelta;
        sizes.x = _width * _nowValue / _fullValue;
        _activeBar.sizeDelta = sizes;

        _isFall = true;
        _barValue.text = _nowValue.ToString();
        _barTooltipValue.text = _nowValue + " / " + _fullValue;
    }

    private void CalculateFallBar()
    {
        _fallValue = Mathf.Lerp(_fallValue, _nowValue, 5 * Time.deltaTime);
        

        if((_nowValue - 0.2f) < _fallValue && (_nowValue + 0.2f) > _fallValue)
        {
            _isFall =false;
            _fallValue = _nowValue;
        }

        var sizes = _fallBar.sizeDelta;
        sizes.x = _width * _fallValue / _fullValue;
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
