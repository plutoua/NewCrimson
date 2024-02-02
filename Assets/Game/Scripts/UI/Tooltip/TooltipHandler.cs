using TimmyFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _tooltipTextCode;

    private string _text;

    private void Start()
    {
        if(Game.IsReady)
        {
            InitialSetup();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void InitialSetup()
    {
        var languageStorage = Game.GetStorage<LanguageStorage>();

        _text = languageStorage.Translation(_tooltipTextCode);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UITooltip.Show(_text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UITooltip.Hide();
    }
}
