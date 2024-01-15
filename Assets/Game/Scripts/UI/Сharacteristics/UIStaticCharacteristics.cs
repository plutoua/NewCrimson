using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class UIStaticCharacteristics : MonoBehaviour
{
    [SerializeField] private UIStaticCharacteristicsItem Health;
    [SerializeField] private UIStaticCharacteristicsItem Stamina;
    [SerializeField] private UIStaticCharacteristicsItem MoveSpeed;
    [SerializeField] private UIStaticCharacteristicsItem AttackSpeed;
    [SerializeField] private UIStaticCharacteristicsItem InventorySize;
    [SerializeField] private UIStaticCharacteristicsItem ViewAngle;
    [SerializeField] private UIStaticCharacteristicsItem ViewLenght;

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
        Health.SetValue(_playerStatController.Stats.Health);
        Stamina.SetValue(_playerStatController.Stats.Stamina);
        MoveSpeed.SetValue(_playerStatController.Stats.MoveSpeed);
        AttackSpeed.SetValue(_playerStatController.Stats.AttackSpeed);
        InventorySize.SetValue(_playerStatController.Stats.InventorySize);
        ViewAngle.SetValue(_playerStatController.Stats.ViewAngle);
        ViewLenght.SetValue(_playerStatController.Stats.ViewLenght);
    }
}
