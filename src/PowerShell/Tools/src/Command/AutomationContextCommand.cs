// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using BlazorFocused.Automation.PowerShell.Session;
using BlazorFocused.Automation.Runtime;

namespace BlazorFocused.Automation.PowerShell.Tools.Command;

internal class AutomationContextCommand<TCommand, TStartup> : AutomationCommand<TCommand>
    where TCommand : PSCmdlet
    where TStartup : IAutomationStartup, new()
{
    public AutomationContextCommand(Action<IServiceCollection> buildServices)
    {
        IServiceCollection services = new ServiceCollection();

        buildServices(services);

        AutomationRuntime.AddRuntimeServices<TStartup>(
            new AutomationSessionStateProxy(powerShellSession.Runspace.SessionStateProxy), services);
    }
}
