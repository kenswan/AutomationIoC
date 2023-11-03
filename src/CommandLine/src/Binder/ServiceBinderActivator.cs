// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorFocused.Automation.CommandLine.Binder;

internal class ServiceBinderActivator : IServiceBinderActivator
{
    private readonly string[] args;
    private readonly IDictionary<string, string> configurationMapping;
    private readonly Action<HostBuilderContext, IServiceCollection> buildServices;
    private readonly Action<HostBuilderContext, IConfigurationBuilder> buildConfiguration;

    private IHost host;

    public ServiceBinderActivator(
        string[] args,
        IDictionary<string, string> configurationMapping,
        Action<HostBuilderContext, IConfigurationBuilder> buildConfiguration,
        Action<HostBuilderContext, IServiceCollection> buildServices)
    {
        this.args = args;
        this.configurationMapping = configurationMapping;
        this.buildConfiguration = buildConfiguration;
        this.buildServices = buildServices;
    }

    public IServiceProvider GetServiceProvider()
    {
        host ??= AutomationRuntime.GenerateRuntimeHost(
            buildConfiguration: buildConfiguration,
            buildServices: buildServices,
            parameters: args,
            parameterConfigurationMappings: configurationMapping);

        return host.Services;
    }
}
