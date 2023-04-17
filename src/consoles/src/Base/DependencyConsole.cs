// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.Consoles.Base;

internal class DependencyConsole : IAutomationIoConsole
{
    private readonly RootCommand rootCommand;

    public DependencyConsole(RootCommand rootCommand)
    {
        this.rootCommand = rootCommand;
    }

    public int Run(string[] args) => rootCommand.Invoke(args);

    public Task<int> RunAsync(string[] args) => rootCommand.InvokeAsync(args);
}
