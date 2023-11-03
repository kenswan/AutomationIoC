// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.Runtime.Binder;
using BlazorFocused.Automation.Runtime.Context;
using BlazorFocused.Automation.Runtime.Dependency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorFocused.Automation.Runtime;

/// <summary>
/// Provides Automation Services needed downstream
/// </summary>
public static class AutomationRuntime
{
    /// <summary>
    /// Binds registered services to context object leveraging dependency injection
    /// </summary>
    /// <typeparam name="TAttribute">Attribute of service that should receive injection</typeparam>
    /// <typeparam name="TStartup">Automation Startup class responsible for establishing dependency injected services</typeparam>
    /// <param name="sessionStorage">Environmental storage used for caching generated services for subsequent calls</param>
    /// <param name="instance">Instance of object that needs to have services injected</param>
    public static void BindServicesByAttribute<TAttribute, TStartup>(ISessionStorage sessionStorage, object instance)
        where TAttribute : Attribute
        where TStartup : IAutomationStartup, new()
    {
        IServiceProvider serviceProvider = DependencyFactory.GenerateServiceProvider(sessionStorage, new TStartup());
        using IServiceScope scope = serviceProvider.CreateScope();

        IAutomationBinder binder = scope.ServiceProvider.GetRequiredService<IAutomationBinder>();

        binder.BindContext<TAttribute>(instance);
    }

    /// <summary>
    /// Binds registered services to context object leveraging dependency injection
    /// </summary>
    /// <typeparam name="TAttribute">Attribute of service that should receive injection</typeparam>
    /// <param name="serviceProvider">Service provider containing services to bind to instance</param>
    /// <param name="instance">Instance of object that needs to have services injected</param>
    public static void BindServicesByAttribute<TAttribute>(IServiceProvider serviceProvider, object instance)
        where TAttribute : Attribute =>
            DependencyBinder.BindServicesByAttribute<TAttribute>(serviceProvider, instance);

    /// <summary>
    /// Adds automation runtime services to an existing service collection to established a cached service provider
    /// for use in downstream automation
    /// </summary>
    /// <typeparam name="TStartup">Automation Startup class responsible for establishing dependency injected services</typeparam>
    /// <param name="sessionState">Environmental storage used for caching generated services for subsequent calls</param>
    /// <param name="servicesOverride">Overrides registered services in automation runtime</param>
    public static void AddRuntimeServices<TStartup>(ISessionStorage sessionState, Action<IServiceCollection> servicesOverride)
        where TStartup : IAutomationStartup, new()
    {
        var startup = new TStartup();

        var contextBuilder = new ContextBuilder(startup, sessionState);

        IServiceCollection serviceCollection = new ServiceCollection()
            .AddScoped<IAutomationBinder, AutomationBinder>()
            .AddSingleton(_ => sessionState)
            .AddSingleton<IAutomationStartup>(_ => startup)
            .AddSingleton<IContextBuilder, ContextBuilder>(_ => contextBuilder);

        contextBuilder.BuildServices(servicesOverride);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    /// <param name="sessionState"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static bool HasRegisteredServiceProvider<TStartup>(ISessionStorage sessionState, out IServiceProvider serviceProvider)
        where TStartup : IAutomationStartup, new()
    {
        serviceProvider = sessionState.GetValue<IHost>(typeof(TStartup).FullName)?.Services;

        return serviceProvider is not null;
    }

    /// <summary>
    /// Generate runtime automation host comprised of given configuration, parameters, and services
    /// </summary>
    /// <param name="buildConfiguration">Used to build application configuration</param>
    /// <param name="buildServices">Used to establish services for dependency injection</param>
    /// <param name="parameters">Command line parameters used to change hosting operation, configuration/environment</param>
    /// <param name="parameterConfigurationMappings">Mappings used to bind parameter values with <see cref="IConfiguration" /> keys</param>
    /// <returns>Runtime automation host with established services</returns>
    public static IHost GenerateRuntimeHost(
        Action<HostBuilderContext, IConfigurationBuilder> buildConfiguration,
        Action<HostBuilderContext, IServiceCollection> buildServices,
        string[] parameters,
        IDictionary<string, string> parameterConfigurationMappings) =>
            DependencyFactory.GenerateHost(buildConfiguration, buildServices, parameters, parameterConfigurationMappings);

    /// <summary>
    /// Generate runtime automation host comprised of given configuration, parameters, and services found in <typeparamref name="TStartup"/>
    /// </summary>
    /// <typeparam name="TStartup">Automation Startup class responsible for establishing dependency injected services</typeparam>
    /// <returns>Runtime automation host with established services</returns>
    public static IHost GenerateRuntimeHost<TStartup>() where TStartup : IAutomationStartup, new()
    {
        var startup = new TStartup();

        return DependencyFactory.GenerateHost(
            startup.Configure,
            startup.ConfigureServices,
            startup.GenerateParameters(),
            startup.GenerateParameterConfigurationMapping());
    }
}
