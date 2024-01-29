using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UISwitchTradeTab : MonoBehaviour
{
    [SerializeField] private CanvasGroup _targetCanvasGroup;

    private Button _button;
    private CanvasGroup _selfCanvasGroup;

    private void Start()
    {
        _button = transform.Find("SelectButton").GetComponent<Button>();
        _button.onClick.AddListener(OnSwitchButtonClick);
        _selfCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnSwitchButtonClick()
    {
        _selfCanvasGroup.alpha = 0;
        _selfCanvasGroup.blocksRaycasts = false;

        _targetCanvasGroup.alpha = 1;
        _targetCanvasGroup.blocksRaycasts = true;
    }
}
