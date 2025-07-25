// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Application;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Builder;

internal class AutomationConsoleBuilder : IAutomationConsoleBuilder
{
    private readonly RootCommand rootCommand;
    private readonly string[] args;

    public AutomationConsoleBuilder(RootCommand rootCommand, string[] args = null)
    {
        this.rootCommand = rootCommand;
        this.args = args;
    }

    public IAutomationConsoleBuilder AddCommand<T>(params string[] commandPath) where T : IConsoleCommand, new()
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
            Command newCommand = internalCommand.Register(addedCommandName, args);

            currentCommand.AddCommand(newCommand);
        }
        // Update existing command if found
        else
        {
            internalCommand.Register(existingCommand, args);
        }

        return this;
    }

    public IAutomationConsole Build() => new AutomationConsoleApplication(rootCommand, args);
}
