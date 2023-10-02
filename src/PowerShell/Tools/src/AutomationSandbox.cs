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
/// Sandbox for running automation commands in isolation
/// </summary>
public static class AutomationSandbox
{
    /// <summary>
    /// Create sandboxed automation context to run automation commands in isolation
    /// </summary>
    /// <typeparam name="TCommand">Type of command used to establish context</typeparam>
    /// <typeparam name="TStartup">Type of service/configuration registration</typeparam>
    /// <param name="buildServices">Service to register for sandboxed instance</param>
    /// <returns>Command instance to run</returns>
    public static IAutomationCommand<TCommand> CreateContext<TCommand, TStartup>(Action<IServiceCollection> buildServices)
        where TCommand : PSCmdlet
        where TStartup : IAutomationStartup, new() =>
            new AutomationContextCommand<TCommand, TStartup>(buildServices);

    /// <summary>
    /// Standard automation command
    /// </summary>
    /// <typeparam name="TCommand">Type of automation command to create instance from</typeparam>
    /// <returns>Command instance to run</returns>
    public static IAutomationCommand<TCommand> CreateCommand<TCommand>()
        where TCommand : PSCmdlet, new() =>
            new AutomationCommand<TCommand>();
}
