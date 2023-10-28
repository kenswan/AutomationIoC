// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Session;
using BlazorFocused.Automation.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell.Tools.Context;

internal class PowerShellAutomationContext<TStartup> : PowerShellAutomationContext, IPowerShellAutomation<TStartup>
    where TStartup : IAutomationStartup, new()
{
    protected readonly string commandName;

    /// <summary>
    /// Create automation session with default service registration from <see cref="IAutomationStartup"/>
    /// </summary>
    public PowerShellAutomationContext() : base()
    {
        string modulePath = typeof(TStartup).Assembly.Location;

        ImportModule(modulePath);
    }

    /// <summary>
    /// Inject services into automation context instead of using default service registration from <see cref="IAutomationStartup"/>
    /// </summary>
    /// <param name="buildServices">Custom service registration</param>
    public PowerShellAutomationContext(Action<IServiceCollection> buildServices) : base()
    {
        IServiceCollection services = new ServiceCollection();

        buildServices(services);

        AutomationRuntime.AddRuntimeServices<TStartup>(
            new AutomationSessionStateProxy(powerShellSession.Runspace.SessionStateProxy), services);
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

    private static string GetCommandName<TCommand>()
        where TCommand : AutomationShell<TStartup> =>
            Attribute.GetCustomAttribute(typeof(TCommand), typeof(CmdletAttribute)) is not CmdletAttribute cmdletAttribute
                ? throw new ArgumentException("CmdletAttribute not found on class", nameof(cmdletAttribute))
                : $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";
}
