// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Tools.Context;
using BlazorFocused.Automation.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Automation.PowerShell.Tools;

/// <summary>
/// Sandbox for running automation commands in isolation
/// </summary>
public static class AutomationSandbox
{
    /// <summary>
    /// Create sandboxed automation context to run automation commands in isolation
    /// </summary>
    /// <typeparam name="TStartup">Type of service/configuration registration</typeparam>
    /// <returns>Command instance to run</returns>
    public static IPowerShellAutomation<TStartup> CreateSession<TStartup>()
        where TStartup : IAutomationStartup, new() =>
            new PowerShellAutomationContext<TStartup>();

    /// <summary>
    /// Create sandboxed automation context to run automation commands in isolation
    /// </summary>
    /// <typeparam name="TStartup">Type of service/configuration registration</typeparam>
    /// <param name="buildServices">Service to register for sandboxed instance</param>
    /// <returns>Automation instance to run</returns>
    public static IPowerShellAutomation<TStartup> CreateSession<TStartup>(Action<IServiceCollection> buildServices)
        where TStartup : IAutomationStartup, new() =>
            new PowerShellAutomationContext<TStartup>(buildServices);

    /// <summary>
    /// Standard automation command
    /// </summary>
    /// <returns>Command instance to run</returns>
    public static IPowerShellAutomation CreateSession() => new PowerShellAutomationContext();
}
