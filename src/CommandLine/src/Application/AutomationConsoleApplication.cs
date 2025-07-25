// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.CommandLine.Application;

internal class AutomationConsoleApplication : IAutomationConsole
{
    private readonly RootCommand rootCommand;
    private readonly string[] arguments;

    public AutomationConsoleApplication(RootCommand rootCommand, string[] arguments = null)
    {
        this.arguments = arguments ?? Environment.GetCommandLineArgs();
        this.rootCommand = rootCommand;
    }

    public int Run() => rootCommand.Invoke(arguments);

    public Task<int> RunAsync() => rootCommand.InvokeAsync(arguments);
}
