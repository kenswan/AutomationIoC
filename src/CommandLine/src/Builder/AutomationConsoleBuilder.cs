// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Application;
using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Builder;

internal class AutomationConsoleBuilder(
    AutomationRootCommand rootCommand,
    AutomationContext automationContext,
    string[]? args = null) : IAutomationConsoleBuilder
{
    public IAutomationConsoleBuilder AddCommand<T>(params string[] commandPath)
        where T : IAutomationCommandInitializer, new()
    {
        string addedCommandName = commandPath.Last();

        Command currentCommand = rootCommand;

        // Traverse to proper parent command
        for (int i = 0; i < commandPath.Length - 1; i++)
        {
            string currentName = commandPath[i];

            Command subCommand = currentCommand.Subcommands.FirstOrDefault(command => command.Name == currentName);

            if (subCommand is null)
            {
                currentCommand.Subcommands.Add(new Command(currentName));

                subCommand = currentCommand.Subcommands.First(command => command.Name == currentName);
            }

            currentCommand = subCommand;
        }

        // Check if command already exists in this path
        // This could happen if the caller registers the command out of order
        // (once in a path, and the other for adding command implementation)
        Command existingCommand =
            currentCommand.Subcommands.FirstOrDefault(command => command.Name == addedCommandName);

        var automationCommandInitializer = new T();

        var newAutomationCommand =
            new AutomationCommand(addedCommandName, automationContext);

        automationCommandInitializer.Initialize(newAutomationCommand);

        // Add new command if not found at proper path as already existing
        if (existingCommand is null)
        {
            currentCommand.Subcommands.Add(newAutomationCommand);
        }
        // Update existing command if found
        else
        {
            AutomationCommand.Clone(source: newAutomationCommand, target: existingCommand);
        }

        return this;
    }

    public IAutomationConsoleBuilder Configure(Action<HostBuilderContext, IConfigurationBuilder> configure)
    {
        automationContext.SetConfigure(configure);
        return this;
    }

    public IAutomationConsoleBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices)
    {
        automationContext.SetConfigureServices(configureServices);
        return this;
    }

    public IAutomationConsoleBuilder WithConfigurationMapping(IDictionary<string, string> mapping)
    {
        automationContext.SetConfigurationMapping(mapping);
        return this;
    }

    public IAutomationConsole Build() => new AutomationConsoleApplication(rootCommand, args);

    internal AutomationRootCommand GetRootCommand() => rootCommand;
}
