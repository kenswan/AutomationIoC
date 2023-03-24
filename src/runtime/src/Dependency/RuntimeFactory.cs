// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Environment;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutomationIoC.Runtime.Dependency;

internal static class RuntimeFactory
{
    public static IServiceProvider RuntimeServiceProvider(ISessionState sessionState, IIoCStartup startup) =>
        GenerateRuntimeDependencies(sessionState)
            .AddTransient<IAutomationIoCBinder, AutomationIoCBinder>()
            .AddTransient<IContextBuilder, ContextBuilder>()
            .AddTransient<ISessionStorageProvider, SessionStorageProvider>()
            .AddTransient(_ => startup)
            .BuildServiceProvider();

    public static IServiceProvider RuntimeServiceProvider(ISessionState sessionState) =>
        GenerateRuntimeDependencies(sessionState)
            .BuildServiceProvider();

    public static void AddClientRuntime(IServiceCollection serviceCollection) =>
        serviceCollection
            .AddTransient<IDependencyBinder, DependencyBinder>()
            .AddLogging(builder => builder.AddConsole());

    private static IServiceCollection GenerateRuntimeDependencies(ISessionState sessionState) =>
        new ServiceCollection()
            .AddTransient<IAutomationEnvironment, AutomationEnvironment>()
            .AddTransient<IEnvironmentStorageProvider, EnvironmentStorageProvider>()
            .AddSingleton<ISessionState>(_ => sessionState);
}
