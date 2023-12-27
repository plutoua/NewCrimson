using TimmyFramework;
using TMPro;
using UnityEngine;

public class CraftUITranslate : MonoBehaviour
{
    [SerializeField] private TMP_Text _craftLabelText;
    [SerializeField] private TMP_Text _craftButtonText;

    private void Start()
    {
        if (Game.IsReady)
        {
            MakeTranslate();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void MakeTranslate()
    {
        var languageStorage = Game.GetStorage<LanguageStorage>();

        _craftLabelText.text = languageStorage.Translation("craft_label_text");
        _craftButtonText.text = languageStorage.Translation("craft_button_text");
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        MakeTranslate();
    }
}
