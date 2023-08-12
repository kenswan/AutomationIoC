// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Binding;

namespace AutomationIoC.Consoles.Binder;
internal class ServiceBinderFactory : IServiceBinderFactory
{
    private readonly ServiceBinderActivator serviceBinderActivator;

    public ServiceBinderFactory(
        string[] args,
        IDictionary<string, string> configurationMapping,
        Action<IConfigurationBuilder> buildConfiguration,
        Action<IServiceCollection> buildServices)
    {
        serviceBinderActivator = new ServiceBinderActivator(
            args,
            configurationMapping,
            buildConfiguration,
            buildServices);
    }

    public BinderBase<T> Bind<T>() => new ServiceBinder<T>(serviceBinderActivator);
}
