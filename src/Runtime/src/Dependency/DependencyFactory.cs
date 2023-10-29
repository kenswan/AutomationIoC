// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.Runtime.Binder;
using BlazorFocused.Automation.Runtime.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorFocused.Automation.Runtime.Dependency;

internal static class DependencyFactory
{
    public static IServiceProvider GenerateServiceProvider(ISessionStorage sessionState, IAutomationStartup startup)
    {
        if (HasRegisteredServiceProvider(sessionState, startup, out IServiceProvider storedServiceProvider))
        {
            return storedServiceProvider;
        }

        IServiceCollection serviceCollection =
            GenerateRuntimeDependencies(sessionState)
            .AddScoped<IAutomationBinder, AutomationBinder>()
            .AddSingleton(_ => startup)
            .AddSingleton<IContextBuilder, ContextBuilder>(_ => new ContextBuilder(startup, sessionState));

        IServiceProvider initialServiceProvider = serviceCollection.BuildServiceProvider();

        using IServiceScope serviceScope = initialServiceProvider.CreateScope();
        IContextBuilder contextBuilder = serviceScope.ServiceProvider.GetRequiredService<IContextBuilder>();

        // Append runtime services to existing service collection
        IServiceProvider serviceProvider = contextBuilder.BuildServices((services) =>
        {
            foreach (ServiceDescriptor service in serviceCollection)
            {
                services.Add(service);
            }
        });

        return serviceProvider;
    }

    public static IServiceProvider GenerateServiceProvider(ISessionStorage sessionStorage) =>
        GenerateRuntimeDependencies(sessionStorage)
            .BuildServiceProvider();

    public static IHost GenerateHost(
        Action<HostBuilderContext, IConfigurationBuilder> buildConfiguration,
        Action<HostBuilderContext, IServiceCollection> buildServices,
        string[] parameters = null,
        IDictionary<string, string> parameterConfigurationMappings = null) => new HostBuilder()
            .ConfigureAppConfiguration((hostBuilderContext, builder) =>
            {
                var additionalSettings = new Dictionary<string, string>()
                {
                    ["BasePath"] = Directory.GetCurrentDirectory(),
                };

                builder
                    .AddEnvironmentVariables()
                    .AddInMemoryCollection(additionalSettings);

                if (buildConfiguration is not null)
                {
                    buildConfiguration(hostBuilderContext, builder);
                }
            })
            // Placing parameters in host configuration allows for other dependencies
            // to be driven off things like "--environment Staging" from the caller
            // which will then be available in HostBuilderContext within
            // "ConfigureAppConfiguration" and "ConfigureServices"
            .ConfigureHostConfiguration((configurationBuilder) =>
            {
                if (parameters is not null)
                {
                    if (parameterConfigurationMappings is not null)
                    {
                        configurationBuilder.AddCommandLine(parameters, parameterConfigurationMappings);
                    }
                    else
                    {
                        configurationBuilder.AddCommandLine(parameters);
                    }
                }
            })
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.AddDebug();
                loggingBuilder.AddConsole();
            })
            .ConfigureServices((hostBuilderContext, services) =>
            {
                if (buildServices is not null)
                {
                    buildServices(hostBuilderContext, services);
                }
            })
            .Build();

    private static IServiceCollection GenerateRuntimeDependencies(ISessionStorage sessionStorage) =>
        new ServiceCollection()
            .AddSingleton(_ => sessionStorage);

    private static bool HasRegisteredServiceProvider(
        ISessionStorage sessionStorage,
        IAutomationStartup startup,
        out IServiceProvider serviceProvider)
    {
        serviceProvider = sessionStorage.GetValue<IHost>(startup.GetType().FullName)?.Services ?? null;

        return serviceProvider is not null;
    }
}
