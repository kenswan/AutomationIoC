// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Session;
using AutomationIoC.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools.Context;

internal class PowerShellAutomationContext<TStartup> : PowerShellAutomationContext, IPowerShellAutomation<TStartup>
    where TStartup : IAutomationStartup, new()
{
    /// <summary>
    /// Create automation session with default service registration from <see cref="IAutomationStartup"/>
    /// </summary>
    public PowerShellAutomationContext() : base()
    {
        ImportPSCmdletModule<TStartup>();
    }

    /// <summary>
    /// Inject services into automation context instead of using default service registration from <see cref="IAutomationStartup"/>
    /// </summary>
    /// <param name="buildServices">Custom service registration</param>
    public PowerShellAutomationContext(Action<IServiceCollection> buildServices) : base()
    {
        ImportPSCmdletModule<TStartup>();

        AutomationRuntime.AddRuntimeServices<TStartup>(
            new AutomationSessionStateProxy(powerShellSession.Runspace.SessionStateProxy), buildServices);
    }

    public ICollection<PSObject> RunAutomationCommand<TCommand>(Action<PSCommand> buildCommand = null)
        where TCommand : AutomationShell<TStartup> =>
            RunAutomationCommand<TCommand, PSObject>(buildCommand);

    public ICollection<TOutput> RunAutomationCommand<TCommand, TOutput>(Action<PSCommand> buildCommand = null)
        where TCommand : AutomationShell<TStartup>
    {
        string commandName = GetCommandName<TCommand>();

        return RunCommand<TOutput>(commandName, buildCommand);
    }
}
