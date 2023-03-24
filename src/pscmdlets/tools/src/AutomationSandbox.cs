// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoc.PSCmdlets.Tools.Command;
using AutomationIoC.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoc.PSCmdlets.Tools;

public static class AutomationSandbox
{
    public static IAutomationCommand<TCommand> CreateContext<TCommand, TStartup>(Action<IServiceCollection> buildServices)
        where TCommand : PSCmdlet
        where TStartup : IIoCStartup, new() =>
            new AutomationContextCommand<TCommand, TStartup>(buildServices);

    public static IAutomationCommand<TCommand> CreateCommand<TCommand>()
        where TCommand : PSCmdlet, new() =>
            new AutomationCommand<TCommand>();
}
