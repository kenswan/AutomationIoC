// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.CommandLine.Binder;

internal class ServiceBinderFactory(IConsoleCommand consoleCommand, string[]? args = null) : IServiceBinderFactory
{
    private readonly ServiceBinderActivator serviceBinderActivator = new(
        args ?? consoleCommand.GenerateParameters(),
        consoleCommand.GenerateParameterConfigurationMapping(),
        consoleCommand.Configure,
        consoleCommand.ConfigureServices);

    public T Bind<T>() => ServiceBinder<T>.GetBoundValue(serviceBinderActivator);
}
