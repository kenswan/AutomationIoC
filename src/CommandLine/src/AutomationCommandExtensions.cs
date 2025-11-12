// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.CommandLine;

/// <summary>
///     Extension methods for building standalone AutomationCommands
/// </summary>
public static class AutomationCommandExtensions
{
    /// <summary>
    ///     Create child (sub) command with initializer
    /// </summary>
    /// <param name="parentCommand">Parent command</param>
    /// <param name="name">Command Line Name to trigger command</param>
    /// <param name="description">Description of command operation</param>
    /// <typeparam name="T">Command Initializer</typeparam>
    /// <returns>New Child Command</returns>
    /// <exception cref="InvalidOperationException">Thrown if parent does not have an established automation context</exception>
    public static AutomationCommand AddCommand<T>(
        this AutomationCommand parentCommand,
        string name,
        string description)
        where T : IAutomationCommandInitializer, new()
    {
        if (parentCommand.Context is null)
        {
            throw new InvalidOperationException("Parent Command is missing automation context.");
        }

        var childAutomationCommand = new AutomationCommand(name, description, parentCommand.Context);
        var commandInitializer = new T();
        commandInitializer.Initialize(childAutomationCommand);
        parentCommand.Subcommands.Add(childAutomationCommand);

        return childAutomationCommand;
    }

    /// <summary>
    ///     Creates child (sub) command without initializer (used for grouping lower command operations)
    /// </summary>
    /// <param name="parentCommand">Parent command</param>
    /// <param name="name">Command Line Name to trigger command</param>
    /// <param name="description">Description of command operation</param>
    /// <returns>New Child Command</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static AutomationCommand AddCommand(
        this AutomationCommand parentCommand,
        string name,
        string description)
    {
        if (parentCommand.Context is null)
        {
            throw new InvalidOperationException("Parent Command is missing automation context.");
        }

        var childAutomationCommand = new AutomationCommand(name, description, parentCommand.Context);
        parentCommand.Subcommands.Add(childAutomationCommand);

        return childAutomationCommand;
    }
}
