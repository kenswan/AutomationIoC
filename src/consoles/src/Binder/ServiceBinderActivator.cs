// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Consoles.Binder;

internal class ServiceBinderActivator : IServiceBinderActivator
{
    private readonly string[] args;
    private readonly IDictionary<string, string> configurationMapping;
    private readonly Action<IServiceCollection> buildServices;
    private readonly Action<IConfigurationBuilder> buildConfiguration;

    private IHost host;

    public ServiceBinderActivator(
        string[] args,
        IDictionary<string, string> configurationMapping,
        Action<IConfigurationBuilder> buildConfiguration,
        Action<IServiceCollection> buildServices)
    {
        this.args = args;
        this.configurationMapping = configurationMapping;
        this.buildConfiguration = buildConfiguration;
        this.buildServices = buildServices;
    }

    public IServiceProvider GetServiceProvider()
    {
        host ??= AutomationIoCRuntime.GenerateRuntimeHost(
            buildConfiguration: buildConfiguration,
            buildServices: buildServices,
            parameters: args,
            parameterConfigurationMappings: configurationMapping);

        return host.Services;
    }
}
