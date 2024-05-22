using System;
using TimmyFramework;

public class BlockerController : IController
{
    public event Action<bool> ConsoleChangeActivityEvent
    {
        add { _consoleChangeActivityEvent += value; }
        remove { _consoleChangeActivityEvent -= value; }
    }

    public event Action<bool> UIChangeActivityEvent
    {
        add { _uiChangeActivityEvent += value; }
        remove { _uiChangeActivityEvent -= value; }
    }

    private event Action<bool> _consoleChangeActivityEvent;
    private event Action<bool> _uiChangeActivityEvent;

    private bool _isUIActive;
    private bool _isConsoleActive;

    public void Initialize()
    {
        _isUIActive = false;
        _isConsoleActive = false;
    }

    public void SetConsole(bool isActive)
    {
        _isConsoleActive = isActive;
        UpdateBlocker();
    }

    public void SetUI(bool isActive)
    {
        _isUIActive = isActive;
        UpdateBlocker();
    }

    private void UpdateBlocker()
    {
        if(_isUIActive || _isConsoleActive)
        {
            _uiChangeActivityEvent?.Invoke(false);
        }
        else
        {
            _uiChangeActivityEvent?.Invoke(true);
        }

        if(_isConsoleActive)
        {
            _consoleChangeActivityEvent?.Invoke(false);
        }
        else
        {
            _consoleChangeActivityEvent?.Invoke(true);
        }
    }
}
