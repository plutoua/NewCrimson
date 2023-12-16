using TMPro;
using UnityEngine;

public class UISliderStayValue : MonoBehaviour
{
    public TMP_Text StayText { get; private set; }

    private void Start()
    {
        StayText = GetComponent<TMP_Text>();
    }
}
