// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Environment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

    public static IHost GenerateRuntimeHost(
        Action<IConfigurationBuilder> buildConfiguration,
        Action<IServiceCollection> buildServices,
        string[] parameters,
        IDictionary<string, string> parameterConfigurationMappings) => new HostBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                var additionalSettings = new Dictionary<string, string>()
                {
                    ["ProjectBasePath"] = Directory.GetCurrentDirectory(),
                };

                builder
                    .AddEnvironmentVariables()
                    .AddInMemoryCollection(additionalSettings);

                if (parameterConfigurationMappings is not null)
                {
                    builder.AddCommandLine(parameters, parameterConfigurationMappings);
                }

                if (buildConfiguration is not null)
                {
                    buildConfiguration(builder);
                }
            })
            .ConfigureLogging(builder =>
            {
                builder.AddDebug();
                builder.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                if (buildServices is not null)
                {
                    buildServices(services);
                }
            })
            .Build();

    public static void SetEnvironment(ISessionState sessionState, string key, object value)
    {
        IServiceProvider runtimeServiceProvider = RuntimeFactory.RuntimeServiceProvider(sessionState);
        using IServiceScope scope = runtimeServiceProvider.CreateScope();

        IEnvironmentStorageProvider environmentStorageProvider = scope.ServiceProvider.GetService<IEnvironmentStorageProvider>();

        environmentStorageProvider.SetEnvironmentVariable(key, value);
    }
}
