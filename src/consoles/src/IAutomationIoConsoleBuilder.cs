// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Consoles;

public interface IAutomationIoConsoleBuilder
{
    IAutomationIoConsoleBuilder AddCommand<T>(params string[] commandPath) where T : ICommand;

    IAutomationIoConsole Build();
}
