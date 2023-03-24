// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime;

public static class AutomationIoCRuntime
{
    public static void BindContext<TAttribute, TStartup>(DependencyContext<TAttribute, TStartup> context)
        where TAttribute : Attribute
        where TStartup : IIoCStartup, new()
    {
        IServiceProvider serviceProvider = RuntimeFactory.RuntimeServiceProvider(context.SessionState, new TStartup());
        using IServiceScope scope = serviceProvider.CreateScope();

        IAutomationIoCBinder binder = scope.ServiceProvider.GetRequiredService<IAutomationIoCBinder>();

        binder.BindContext<TAttribute>(context.Instance);
    }

    public static void BuildServices<TStartup>(ISessionState sessionState, IServiceCollection serviceCollection)
        where TStartup : IIoCStartup, new()
    {
        IServiceProvider serviceProvider = RuntimeFactory.RuntimeServiceProvider(sessionState, new TStartup());
        using IServiceScope scope = serviceProvider.CreateScope();

        IContextBuilder contextBuilder = scope.ServiceProvider.GetRequiredService<IContextBuilder>();

        contextBuilder.BuildServices(serviceCollection);
    }

    public static void SetEnvironment(ISessionState sessionState, string key, object value)
    {
        IServiceProvider runtimeServiceProvider = RuntimeFactory.RuntimeServiceProvider(sessionState);
        using IServiceScope scope = runtimeServiceProvider.CreateScope();

        IEnvironmentStorageProvider environmentStorageProvider = scope.ServiceProvider.GetService<IEnvironmentStorageProvider>();

        environmentStorageProvider.SetEnvironmentVariable(key, value);
    }
}
