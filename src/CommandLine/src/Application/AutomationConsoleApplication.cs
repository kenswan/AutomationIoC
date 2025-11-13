// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.CommandLine.Application;

internal class AutomationConsoleApplication(
    AutomationRootCommand rootCommand,
    string[]? arguments = null) : IAutomationConsole
{
    private readonly string[] arguments = arguments ?? Environment.GetCommandLineArgs();

    public int Run() => rootCommand.Parse(arguments).Invoke();

    public Task<int> RunAsync(CancellationToken cancellationToken = default) =>
        rootCommand.Parse(arguments).InvokeAsync(cancellationToken: cancellationToken);
}
