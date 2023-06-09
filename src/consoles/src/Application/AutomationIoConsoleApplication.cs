// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.Consoles.Application;

internal class AutomationIoConsoleApplication : IAutomationIoConsole
{
    private readonly RootCommand rootCommand;
    private readonly string[] arguments;

    public AutomationIoConsoleApplication(RootCommand rootCommand, string[] arguments)
    {
        this.rootCommand = rootCommand;
        this.arguments = arguments;
    }

    public int Run() => rootCommand.Invoke(arguments);

    public Task<int> RunAsync() => rootCommand.InvokeAsync(arguments);
}
