// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine.Binding;

namespace AutomationIoC.CommandLine.Binder;

internal class ServiceBinderFactory : IServiceBinderFactory
{
    private readonly ServiceBinderActivator serviceBinderActivator;

    public ServiceBinderFactory(IConsoleCommand consoleCommand, string[] args = null)
    {
        serviceBinderActivator = new ServiceBinderActivator(
            args ?? consoleCommand.GenerateParameters(),
            consoleCommand.GenerateParameterConfigurationMapping(),
            consoleCommand.Configure,
            consoleCommand.ConfigureServices);
    }

    public BinderBase<T> Bind<T>() => new ServiceBinder<T>(serviceBinderActivator);
}
