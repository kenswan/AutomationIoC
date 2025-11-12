// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime;

/// <summary>
///     Creates and manages the service provider and configuration for the current automation process.
/// </summary>
public sealed class AutomationContext : IAutomationContext
{
    private readonly AutomationServiceActivator automationServiceActivator = new();

    /// <inheritdoc />
    public IConfiguration Configuration => this.ServiceProvider.GetRequiredService<IConfiguration>();

    /// <inheritdoc />
    public IServiceProvider ServiceProvider => automationServiceActivator.GetServiceProvider();

    /// <summary>
    ///     Sets the command line arguments to be used for configuration.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    public void SetArgs(string[] args) =>
        automationServiceActivator.SetArgs(args);

    /// <summary>
    ///     Sets the configuration builder action.
    /// </summary>
    /// <param name="configure">Builds <see cref="Configuration" /> property</param>
    public void SetConfigure(Action<HostBuilderContext, IConfigurationBuilder> configure) =>
        automationServiceActivator.SetConfiguration(configure);

    /// <summary>
    ///     Sets the service collection configuration action.
    /// </summary>
    /// <param name="configureServices">Builds <see cref="ServiceProvider" /></param>
    public void SetConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices) =>
        automationServiceActivator.SetServices(configureServices);

    /// <summary>
    ///     Sets the mapping of command line arguments to configuration keys.
    /// </summary>
    /// <param name="configurationMapping">Configuration key mapping</param>
    public void SetConfigurationMapping(IDictionary<string, string> configurationMapping) =>
        automationServiceActivator.SetConfigurationMapping(configurationMapping);
}
