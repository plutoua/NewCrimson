using System.Collections;
using TimmyFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Transform _consoleContentTransform;
    [SerializeField] private TMP_Text _consoleTextPrefab;
    [SerializeField] private ScrollRect _scrollRect;

    private ConsoleController _console;
    private BlockerController _blocker;
    private CanvasGroup _canvasGroup;
    private bool _isVisible;

    private void Start()
    {
        _inputField.onSubmit.AddListener(OnInputEnd);
        _canvasGroup = GetComponent<CanvasGroup>();
        _isVisible = false;

        if (Game.IsReady)
        {
            OnGameReady();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            SwichVisibility();
        }
        if(_isVisible && Input.GetKeyDown(KeyCode.Escape))
        {
            HideConsole();
        }
    }

    private void OnInputEnd(string value)
    {
        var consoleItem = Instantiate(_consoleTextPrefab, _consoleContentTransform);
        consoleItem.text = value;
        ResetConsole();
        DoAction(value);

        StartCoroutine(SetScrollPositionToEnd());
    }

    private IEnumerator SetScrollPositionToEnd()
    {
        yield return null;
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        _console = Game.GetController<ConsoleController>();
        _blocker = Game.GetController<BlockerController>();
    }

    private void DoAction(string value)
    {
        var args = value.Split(" ");
        

        if(args.Length >= 4 ) 
        {
            _console.DoAction(args[0], args[1], args[2], args[3]);
        }
        else if(args.Length == 3 ) 
        {
            _console.DoAction(args[0], args[1], args[2], string.Empty);
        }
        else if (args.Length == 2)
        {
            _console.DoAction(args[0], args[1], string.Empty, string.Empty);
        }
        else if (args.Length == 1)
        {
            _console.DoAction(args[0], string.Empty, string.Empty, string.Empty);
        }
    }

    private void SwichVisibility()
    {
        if (_isVisible)
        {
            HideConsole();
        }
        else
        {
            _isVisible = true;
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _blocker.SetConsole(_isVisible);
        }  
    }

    private void HideConsole()
    {
        _isVisible = false;
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        ResetConsole();
        _blocker.SetConsole(_isVisible);
    }

    private void ResetConsole()
    {
        _inputField.text = string.Empty;
    }
}
