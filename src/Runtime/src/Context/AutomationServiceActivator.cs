// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime.Context;

internal class AutomationServiceActivator
{
    private string[] args = [];
    private Action<HostBuilderContext, IConfigurationBuilder> buildConfiguration = (context, builder) => { };
    private Action<HostBuilderContext, IServiceCollection> buildServices = (context, services) => { };
    private IDictionary<string, string> configurationMapping = new Dictionary<string, string>();

    private IHost? host;

    internal IServiceProvider GetServiceProvider()
    {
        host ??= AutomationRuntime.GenerateRuntimeHost(
            buildConfiguration: buildConfiguration,
            buildServices: buildServices,
            parameters: args,
            parameterConfigurationMappings: configurationMapping);

        return host.Services;
    }

    public void SetArgs(string[] args)
    {
        host = null;
        this.args = args;
    }

    public void SetConfiguration(Action<HostBuilderContext, IConfigurationBuilder> buildConfiguration)
    {
        host = null;
        this.buildConfiguration = buildConfiguration;
    }

    public void SetServices(Action<HostBuilderContext, IServiceCollection> buildServices)
    {
        host = null;
        this.buildServices = buildServices;
    }

    public void SetConfigurationMapping(IDictionary<string, string> configurationMapping)
    {
        host = null;
        this.configurationMapping = configurationMapping;
    }
}
