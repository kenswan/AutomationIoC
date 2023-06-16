// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Consoles.Builder;
using System.CommandLine;

namespace AutomationIoC.Consoles;

public class AutomationIoConsole
{
    public static IAutomationIoConsoleBuilder CreateDefaultBuilder(string[] args, string appDescription = null)
    {
        var rootCommand = new RootCommand(appDescription ?? string.Empty);

        return new AutomationIoConsoleBuilder(rootCommand, args);
    }

    public static IAutomationIoConsoleBuilder CreateDefaultBuilder<T>(string[] args, string appDescription = null)
        where T : IConsoleCommand, new()
    {
        RootCommand rootCommand = new T().Register(args);

        rootCommand.Description = appDescription ?? string.Empty;

        return new AutomationIoConsoleBuilder(rootCommand, args);
    }
}
