// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;
using BlazorFocused.Automation.Runtime;

namespace BlazorFocused.Automation.CommandLine;

/// <summary>
/// Default command implementation used to register commands
/// </summary>
public abstract class StandardCommand : AutomationStartup, IConsoleCommand
{
    /// <summary>
    /// Command name used for logging and help menu
    /// </summary>
    public string ApplicationName => RootCommand.ExecutableName;

    /// <inheritdoc />
    public abstract void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command);
}
