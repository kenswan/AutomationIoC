// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.PSCmdlets.Tools.Command;

internal class AutomationContextCommand<TCommand, TStartup> : AutomationCommand<TCommand>
    where TCommand : PSCmdlet
    where TStartup : IIoCStartup, new()
{
    public AutomationContextCommand(Action<IServiceCollection> buildServices)
    {
        IServiceCollection services = new ServiceCollection();

        buildServices(services);

        AutomationIoCRuntime.BuildServices<TStartup>(powerShellSession.Runspace.SessionStateProxy, services);
    }
}
