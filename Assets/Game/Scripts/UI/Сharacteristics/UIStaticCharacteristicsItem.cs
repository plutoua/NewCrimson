using TMPro;
using UnityEngine;

public class UIStaticCharacteristicsItem : MonoBehaviour
{
    private TMP_Text _valueText;

    public void SetValue(int value)
    {
        _valueText.text = value.ToString();
    }

    public void SetValue(float value)
    {
        _valueText.text = value.ToString();
    }

    private void Start()
    {
        _valueText = transform.Find("Value").GetComponent<TMP_Text>();
    }
}
