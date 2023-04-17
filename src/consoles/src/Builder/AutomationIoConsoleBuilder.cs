// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Consoles.Builder;

internal class AutomationIoConsoleBuilder : IAutomationIoConsoleBuilder
{
    public string AppName { get; private set; }

    public AutomationIoConsoleBuilder(string appName)
    {
        AppName = appName;
    }

    public IAutomationIoConsoleBuilder AddCommand<T>(string[] commandPath) where T : ICommand => throw new NotImplementedException();

    public IAutomationIoConsole Build() => throw new NotImplementedException();
}
