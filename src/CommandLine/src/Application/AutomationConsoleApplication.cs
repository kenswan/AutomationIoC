// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.CommandLine.Application;

internal class AutomationConsoleApplication(
    AutomationCommand rootCommand,
    string[]? arguments = null) : IAutomationConsole
{
    private readonly string[] arguments = arguments ?? Environment.GetCommandLineArgs();

    public int Run() => new CommandLineConfiguration(rootCommand).Invoke(arguments);

    public Task<int> RunAsync(CancellationToken cancellationToken = default) =>
        new CommandLineConfiguration(rootCommand).InvokeAsync(arguments, cancellationToken);
}
