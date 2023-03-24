// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Environment;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using Runspace = System.Management.Automation.Runspaces;

namespace AutomationIoC.Runtime;

public static class AutomationIoCRuntime
{
    public static void BindContext<TAttribute, TStartup>(DependencyContext<TAttribute, TStartup> context)
        where TAttribute : Attribute
        where TStartup : IIoCStartup, new()
    {
        var sessionStateProxy = new SessionStateProxy(context.SessionState);

        IServiceProvider serviceProvider = RuntimeFactory.RuntimeServiceProvider(sessionStateProxy, new TStartup());
        using IServiceScope scope = serviceProvider.CreateScope();

        IAutomationIoCBinder binder = scope.ServiceProvider.GetRequiredService<IAutomationIoCBinder>();

        binder.BindContext<TAttribute>(context.Instance);
    }

    public static void BuildServices<TStartup>(Runspace.SessionStateProxy sessionStateProxy, IServiceCollection serviceCollection)
        where TStartup : IIoCStartup, new()
    {
        var automationSessionStateProxy = new SessionStateProxy(sessionStateProxy);

        IServiceProvider serviceProvider = RuntimeFactory.RuntimeServiceProvider(automationSessionStateProxy, new TStartup());
        using IServiceScope scope = serviceProvider.CreateScope();

        IContextBuilder contextBuilder = scope.ServiceProvider.GetRequiredService<IContextBuilder>();

        contextBuilder.BuildServices(serviceCollection);
    }

    public static void SetEnvironment(SessionState sessionState, string key, object value)
    {
        var sessionStateProxy = new SessionStateProxy(sessionState);

        IServiceProvider runtimeServiceProvider = RuntimeFactory.RuntimeServiceProvider(sessionStateProxy);
        using IServiceScope scope = runtimeServiceProvider.CreateScope();

        IEnvironmentStorageProvider environmentStorageProvider = scope.ServiceProvider.GetService<IEnvironmentStorageProvider>();

        environmentStorageProvider.SetEnvironmentVariable(key, value, ScopedItemOptions.None);
    }
}
