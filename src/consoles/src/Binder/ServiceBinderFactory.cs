// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine.Binding;

namespace AutomationIoC.Consoles.Binder;
internal class ServiceBinderFactory : IServiceBinderFactory
{
    private readonly string[] args;
    private readonly IDictionary<string, string> configurationMapping;
    private readonly Action<IServiceCollection> buildServices;
    private readonly Action<IConfigurationBuilder> buildConfiguration;

    private IHost host;

    public ServiceBinderFactory(
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

    public BinderBase<T> Create<T>()
    {
        host ??= AutomationIoCRuntime.GenerateRuntimeHost(
            buildConfiguration: buildConfiguration,
            buildServices: buildServices,
            parameters: args,
            parameterConfigurationMappings: configurationMapping);

        return new ServiceBinder<T>(host.Services);
    }
}
