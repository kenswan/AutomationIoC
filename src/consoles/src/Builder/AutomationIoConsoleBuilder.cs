// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Consoles.Application;
using System.CommandLine;

namespace AutomationIoC.Consoles.Builder;

internal class AutomationIoConsoleBuilder : IAutomationIoConsoleBuilder
{
    private readonly RootCommand rootCommand;
    private readonly string[] arguments;

    public AutomationIoConsoleBuilder(RootCommand rootCommand, string[] arguments)
    {
        this.rootCommand = rootCommand;
        this.arguments = arguments;
    }

    public IAutomationIoConsoleBuilder AddCommand<T>(params string[] commandPath) where T : ICommand, new()
    {
        string addedCommandName = commandPath.Last();

        Command currentCommand = this.rootCommand;

        // Traverse to proper parent command
        for (int i = 0; i < commandPath.Length - 1; i++)
        {
            string currentName = commandPath[i];

            Command subCommand = currentCommand.Subcommands.FirstOrDefault(command => command.Name == currentName);

            if (subCommand is null)
            {
                currentCommand.AddCommand(new Command(currentName));

                subCommand = currentCommand.Subcommands.First(command => command.Name == currentName);
            }

            currentCommand = subCommand;
        }

        // Check if command already exists in this path
        // This could happen if the caller registers the command out of order
        // (once in a path, and the other for adding command implementation)
        Command existingCommand = currentCommand.Subcommands.FirstOrDefault(command => command.Name == addedCommandName);

        var internalCommand = new T();

        // Add new command if not found at proper path as already existing
        if (existingCommand is null)
        {
            Command newCommand = internalCommand.Register(addedCommandName, arguments);

            currentCommand.AddCommand(newCommand);
        }
        // Update existing command if found
        else
        {
            internalCommand.Register(existingCommand, arguments);
        }

        return this;
    }

    public IAutomationIoConsole Build() => new AutomationIoConsoleApplication(rootCommand, arguments);
}
