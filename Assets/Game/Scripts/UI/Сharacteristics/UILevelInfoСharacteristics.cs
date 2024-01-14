using TimmyFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelInfoСharacteristics : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _experienceText;
    [SerializeField] private Slider _experienceBar;

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
        _playerStatController.PlayerLevelUpEvent += OnLevelChange;
        _playerStatController.PlayerExperienceChangeEvent += OnExperienceChange;

        OnLevelChange();
        OnExperienceChange();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void OnLevelChange()
    {
        _levelText.text = "Level " + _playerStatController.Stats.Level;
    }

    private void OnExperienceChange()
    {
        _experienceText.text = _playerStatController.Stats.Experience + " / " + _playerStatController.Stats.ExperienceBorder;
        _experienceBar.value = _playerStatController.Stats.Experience / (float)_playerStatController.Stats.ExperienceBorder;
    }
}
