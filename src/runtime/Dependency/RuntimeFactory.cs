using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Environment;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutomationIoC.Runtime.Dependency;

internal static class RuntimeFactory
{
    public static IServiceProvider RuntimeServiceProvider(SessionStateProxy sessionState, IIoCStartup startup) =>
        GenerateRuntimeDependencies(sessionState)
            .AddTransient<IAutomationIoCBinder, AutomationIoCBinder>()
            .AddTransient<IContextBuilder, ContextBuilder>()
            .AddTransient<ISessionStorageProvider, SessionStorageProvider>()
            .AddTransient(_ => startup)
            .BuildServiceProvider();

    public static IServiceProvider RuntimeServiceProvider(SessionStateProxy sessionState) =>
        GenerateRuntimeDependencies(sessionState)
            .BuildServiceProvider();

    public static void AddClientRuntime(IServiceCollection serviceCollection) =>
        serviceCollection
            .AddTransient<IDependencyBinder, DependencyBinder>()
            .AddLogging(builder => builder.AddConsole());

    private static IServiceCollection GenerateRuntimeDependencies(SessionStateProxy sessionState) =>
        new ServiceCollection()
            .AddTransient<IAutomationEnvironment, AutomationEnvironment>()
            .AddTransient<IEnvironmentStorageProvider, EnvironmentStorageProvider>()
            .AddSingleton<ISessionState>(_ => sessionState);
}
