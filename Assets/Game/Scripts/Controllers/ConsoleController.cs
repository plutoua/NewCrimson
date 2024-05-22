using System;
using System.Collections.Generic;
using TimmyFramework;

public class ConsoleController : IController
{
    public event Action<string> OnNeedWriteToConsoleEvent
    {
        add { _onNeedWriteToConsoleEvent += value; }
        remove { _onNeedWriteToConsoleEvent -= value; }
    }

    private event Action<string> _onNeedWriteToConsoleEvent;

    private Dictionary<string, Action<string, string, string>> _commands;
    public void Initialize()
    {
        _commands = new Dictionary<string, Action<string, string, string>>();
    }

    public void AddCommand(string command, Action<string, string, string> action)
    {
        command = command.ToLower();
        if (!_commands.ContainsKey(command))
        {
            _commands.Add(command, action);
        }
    }

    public void DoAction(string command, string arg1, string arg2, string arg3)
    {
        command = command.ToLower();
        if (_commands.ContainsKey(command))
        {
            _commands[command](arg1, arg2, arg3);
        }
           
    }

    public void WriteToConsole(string message)
    {
        _onNeedWriteToConsoleEvent?.Invoke(message);
    }
}
