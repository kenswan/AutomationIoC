// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools;

/// <summary>
/// PowerShell automation command interface based on <see cref="IAutomationStartup"/> dependency injection
/// </summary>
/// <typeparam name="TStartup"><see cref="IAutomationStartup"/> used to wire up service dependencies and configuration</typeparam>
public interface IPowerShellAutomation<TStartup> : IPowerShellAutomation
    where TStartup : IAutomationStartup, new()
{
    /// <summary>
    /// Run <see cref="AutomationShell{TStartup}"/> command
    /// </summary>
    /// <param name="buildCommand">Add additional properties and parameters to PowerShell command being run</param>
    /// <returns>Response object returned from isolated PowerShell process</returns>
    ICollection<PSObject> RunAutomationCommand<TCommand>(Action<PSCommand> buildCommand = null)
        where TCommand : AutomationShell<TStartup>;

    /// <summary>
    /// Run <see cref="AutomationShell{TStartup}"/> command with deserialized output
    /// </summary>
    /// <typeparam name="TCommand"><see cref="AutomationShell{TStartup}"/> command used to run operation</typeparam>
    /// <typeparam name="TOutput">Type of output expected</typeparam>
    /// <param name="buildCommand">Add additional properties and parameters to PowerShell command being run</param>
    /// <returns>Deserialized response object returned from isolated PowerShell process</returns>
    ICollection<TOutput> RunAutomationCommand<TCommand, TOutput>(Action<PSCommand> buildCommand = null)
        where TCommand : AutomationShell<TStartup>;
}
