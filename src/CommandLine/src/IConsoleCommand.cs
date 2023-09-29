// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;
using BlazorFocused.Automation.Runtime;

namespace BlazorFocused.Automation.CommandLine;

/// <summary>
///
/// </summary>
public interface IConsoleCommand : IAutomationStartup
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="serviceBinderFactory"></param>
    /// <param name="command"></param>
    void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command);
}
