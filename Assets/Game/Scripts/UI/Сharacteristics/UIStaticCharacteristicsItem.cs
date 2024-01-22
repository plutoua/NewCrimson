using TimmyFramework;
using TMPro;
using UnityEngine;

public class UIStaticCharacteristicsItem : MonoBehaviour
{
    [SerializeField] private Stat _stat;

    private TMP_Text _valueText;
    private PlayerStatController _playerStatController;

    private void Start()
    {
        _valueText = transform.Find("Value").GetComponent<TMP_Text>();

        if (Game.IsReady)
        {
            InitialSetup();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void InitialSetup()
    {
        _playerStatController = Game.GetController<PlayerStatController>();
        _playerStatController.PlayerStatChangeEvent += OnStatChange;

        OnStatChange();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void OnStatChange()
    {
        _valueText.text = _playerStatController.GetStat(_stat).ToString();
    }
}
