// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BlazorFocused.Automation.Runtime.Binder;
using BlazorFocused.Automation.Runtime.Context;

namespace BlazorFocused.Automation.Runtime.Dependency;

internal static class DependencyFactory
{
    public static IServiceProvider GenerateServiceProvider(ISessionStorage sessionState, IAutomationStartup startup) =>
        GenerateRuntimeDependencies(sessionState)
            .AddScoped<IAutomationBinder, AutomationBinder>()
            .AddScoped<IContextBuilder, ContextBuilder>()
            .AddScoped(_ => startup)
            .BuildServiceProvider();

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
}
