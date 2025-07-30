// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime.Context;

internal class AutomationContext : IAutomationContext
{
    private readonly AutomationServiceActivator automationServiceActivator = new();

    public IConfiguration Configuration => this.ServiceProvider.GetRequiredService<IConfiguration>();

    public IServiceProvider ServiceProvider => automationServiceActivator.GetServiceProvider();

    public void SetArgs(string[] args) =>
        automationServiceActivator.SetArgs(args);

    public void SetConfigure(Action<HostBuilderContext, IConfigurationBuilder> configure) =>
        automationServiceActivator.SetConfiguration(configure);

    public void SetConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices) =>
        automationServiceActivator.SetServices(configureServices);

    public void SetConfigurationMapping(IDictionary<string, string> configurationMapping) =>
        automationServiceActivator.SetConfigurationMapping(configurationMapping);
}
