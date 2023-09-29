// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using BlazorFocused.Automation.PowerShell.Tools.Command;
using BlazorFocused.Automation.Runtime;

namespace BlazorFocused.Automation.PowerShell.Tools;

/// <summary>
///
/// </summary>
public static class AutomationSandbox
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TStartup"></typeparam>
    /// <param name="buildServices"></param>
    /// <returns></returns>
    public static IAutomationCommand<TCommand> CreateContext<TCommand, TStartup>(Action<IServiceCollection> buildServices)
        where TCommand : PSCmdlet
        where TStartup : IAutomationStartup, new() =>
            new AutomationContextCommand<TCommand, TStartup>(buildServices);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns></returns>
    public static IAutomationCommand<TCommand> CreateCommand<TCommand>()
        where TCommand : PSCmdlet, new() =>
            new AutomationCommand<TCommand>();
}
