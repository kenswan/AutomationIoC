// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;
using BlazorFocused.Automation.CommandLine.Builder;

namespace BlazorFocused.Automation.CommandLine;

/// <summary>
///
/// </summary>
public class AutomationConsole
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="appDescription"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IAutomationConsoleBuilder CreateDefaultBuilder(string appDescription = null, string[] args = null)
    {
        var rootCommand = new RootCommand(appDescription ?? string.Empty);

        return new AutomationConsoleBuilder(rootCommand, args);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="appDescription"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IAutomationConsoleBuilder CreateDefaultBuilder<T>(string appDescription = null, string[] args = null)
        where T : IConsoleCommand, new()
    {
        RootCommand rootCommand = new T().Register();

        rootCommand.Description = appDescription ?? string.Empty;

        return new AutomationConsoleBuilder(rootCommand, args);
    }
}
