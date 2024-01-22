using TimmyFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatItem : MonoBehaviour
{
    [SerializeField] private Stat _stat;

    private Button _addButton;
    private TMP_Text _valueText;
    private PlayerStatController _playerStatController;

    private void Start()
    {
        _addButton = transform.Find("Add").GetComponent<Button>();
        _addButton.onClick.AddListener(OnAddButtonClick);
        _addButton.gameObject.SetActive(false);

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
        _playerStatController.PlayerLevelUpEvent += OnChangeStat;
        _playerStatController.PlayerStatChangeEvent += OnChangeStat;
        GetValue();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void OnChangeStat()
    {
        if(_playerStatController.Stats.StatsPoints == 0)
        {
            _addButton.gameObject.SetActive(false);
        }
        else
        {
            _addButton.gameObject.SetActive(true);
        }
        GetValue();
    }

    private void OnAddButtonClick()
    {
        _playerStatController.AddStatsByPoint(_stat, 1);
    }

    private void GetValue()
    {
        _valueText.text = _playerStatController.GetStat(_stat).ToString();
    }
}
