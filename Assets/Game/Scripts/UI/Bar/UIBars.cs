using TimmyFramework;
using UnityEngine;

public class UIBars : MonoBehaviour
{
    [SerializeField] private BarFiller _healthBar;
    [SerializeField] private BarFiller _staminaBar;

    private PlayerStatController _playerStatController;

    private void Start()
    {
        if (Game.IsReady)
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
        _playerStatController = Game.GetController<PlayerStatController>();

        _healthBar.SetMaxValue(_playerStatController.GetStat(Stat.Health));
        _healthBar.SetNowValue(_playerStatController.Health);

        _staminaBar.SetMaxValue(_playerStatController.GetStat(Stat.Stamina));
        _staminaBar.SetNowValue(_playerStatController.Stamina);

        _playerStatController.PlayerHealthChangeEvent += OnHealthChange;
        _playerStatController.StaminaHealthChangeEvent += OnStaminaChange;
        _playerStatController.PlayerStatChangeEvent += OnStatChange;
    }

    private void OnHealthChange(int value)
    {
        _healthBar.SetNowValue(value);
    }

    private void OnStaminaChange(int value)
    {
        _staminaBar.SetNowValue(value);
    }

    private void OnStatChange()
    {
        _healthBar.SetMaxValue(_playerStatController.GetStat(Stat.Health));
        _staminaBar.SetMaxValue(_playerStatController.GetStat(Stat.Stamina));
    }
}
