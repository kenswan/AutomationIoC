// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Consoles.Binder;
using System.CommandLine;

namespace AutomationIoC.Consoles.Base;

internal static class CommandExtensions
{
    internal static RootCommand Register(this ICommand internalCommand, string[] args)
    {
        var rootCommand = new RootCommand();

        internalCommand.Register(rootCommand, args);

        return rootCommand;
    }

    internal static Command Register(this ICommand internalCommand, string name, string[] args)
    {
        var command = new Command(name);

        internalCommand.Register(command, args);

        return command;
    }

    internal static void Register(this ICommand internalCommand, Command operationalCommand, string[] args)
    {
        IServiceBinderFactory serviceBinderFactory = InitializeServiceBinderFactory(internalCommand, args);

        internalCommand.ConfigureCommand(serviceBinderFactory, operationalCommand);
    }

    private static IServiceBinderFactory InitializeServiceBinderFactory(ICommand command, string[] args) =>
        new ServiceBinderFactory(args, command.ConfigurationMapping, command.ConfigurationBuilder, command.Services);
}
