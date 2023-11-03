// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.CommandLine.Binder;
using System.CommandLine;

namespace BlazorFocused.Automation.CommandLine.Builder;

internal static class ConsoleCommandExtensions
{
    internal static RootCommand Register(this IConsoleCommand internalCommand, string[] args = null)
    {
        var rootCommand = new RootCommand();

        internalCommand.Register(rootCommand, args);

        return rootCommand;
    }

    internal static Command Register(this IConsoleCommand internalCommand, string name, string[] args = null)
    {
        var command = new Command(name);

        internalCommand.Register(command, args);

        return command;
    }

    internal static void Register(this IConsoleCommand internalCommand, Command operationalCommand, string[] args = null)
    {
        IServiceBinderFactory serviceBinderFactory = InitializeServiceBinderFactory(internalCommand, args);

        internalCommand.ConfigureCommand(serviceBinderFactory, operationalCommand);
    }

    private static IServiceBinderFactory InitializeServiceBinderFactory(IConsoleCommand consoleCommand, string[] args = null) =>
            new ServiceBinderFactory(consoleCommand, args);
}
