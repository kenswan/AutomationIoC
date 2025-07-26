// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Binder;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Builder;

internal static class ConsoleCommandExtensions
{
    internal static RootCommand Register(this IConsoleCommand internalCommand, string[]? args = null)
    {
        var rootCommand = new RootCommand();

        internalCommand.Register(rootCommand, args);

        return rootCommand;
    }

    internal static Command Register(
        this IConsoleCommand internalCommand,
        string name,
        string[]? args = null)
    {
        var command = new Command(name);

        internalCommand.Register(command, args);

        return command;
    }

    internal static void Register(
        this IConsoleCommand internalCommand,
        Command operationalCommand,
        string[]? args = null)
    {
        IServiceBinderFactory serviceBinderFactory = new ServiceBinderFactory(internalCommand, args);

        internalCommand.ConfigureCommand(serviceBinderFactory, operationalCommand);
    }

    internal static RootCommand Register(
        this IConsoleCommand internalCommand,
        IServiceBinderFactory serviceBinderFactory)
    {
        var rootCommand = new RootCommand();

        internalCommand.Register(rootCommand, serviceBinderFactory);

        return rootCommand;
    }

    internal static Command Register(
        this IConsoleCommand internalCommand,
        string name,
        IServiceBinderFactory serviceBinderFactory)
    {
        var command = new Command(name);

        internalCommand.Register(command, serviceBinderFactory);

        return command;
    }

    internal static void Register(
        this IConsoleCommand internalCommand,
        Command operationalCommand,
        IServiceBinderFactory serviceBinderFactory) =>
        internalCommand.ConfigureCommand(serviceBinderFactory, operationalCommand);
}
