// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Consoles.Binder;
using System.CommandLine;

namespace AutomationIoC.Consoles.Builder;

internal static class ConsoleCommandExtensions
{
    internal static RootCommand Register(this IConsoleCommand internalCommand, string[] args)
    {
        var rootCommand = new RootCommand();

        internalCommand.Register(rootCommand, args);

        return rootCommand;
    }

    internal static Command Register(this IConsoleCommand internalCommand, string name, string[] args)
    {
        var command = new Command(name);

        internalCommand.Register(command, args);

        return command;
    }

    internal static void Register(this IConsoleCommand internalCommand, Command operationalCommand, string[] args)
    {
        IServiceBinderFactory serviceBinderFactory = InitializeServiceBinderFactory(internalCommand, args);

        internalCommand.ConfigureCommand(serviceBinderFactory, operationalCommand);
    }

    private static IServiceBinderFactory InitializeServiceBinderFactory(IConsoleCommand command, string[] args) =>
        new ServiceBinderFactory(args, command.ConfigurationMapping, command.ConfigurationBuilder, command.Services);
}
