using TMPro;
using UnityEngine;

public class UISliderSendValue : MonoBehaviour
{
    public TMP_Text SendText { get; private set; }

    private void Start()
    {
        SendText = GetComponent<TMP_Text>();
    }
}
