﻿// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using System.CommandLine;

namespace AutomationIoC.CommandLine;

/// <summary>
///     Extended command class with built-in dependency injection support for actions.
/// </summary>
/// <param name="name">Name of command</param>
/// <param name="description">Command description (available in help menu)</param>
/// <param name="automationContext">Automation Services and configurations</param>
public class AutomationCommand(
    string name,
    string? description,
    IAutomationContext automationContext) : Command(name, description)
{
    /// <summary>
    ///     Set the action to be executed when the command is invoked (synchronous).
    ///     The action receives the parsed result and the automation context, allowing for dependency injection.
    /// </summary>
    /// <param name="action">Dependency Injection Action for synchronous command invocation</param>
    public void SetAction(Action<ParseResult, IAutomationContext> action) =>
        SetAction(parsedResult => action(parsedResult, automationContext));

    /// <summary>
    ///     Set the action to be executed when the command is invoked (asynchronous).
    ///     The action receives the parsed result and the automation context, allowing for dependency injection.
    /// </summary>
    /// <param name="action">Dependency Injection Action for asynchronous command invocation</param>
    public void SetAction(Func<ParseResult, IAutomationContext, CancellationToken, Task> action) =>
        SetAction(async (parsedResult, cancellationToken) =>
        {
            await action(parsedResult, automationContext, cancellationToken).ConfigureAwait(false);
        });

    internal static void Clone(
        Command source,
        Command target)
    {
        target.Description = source.Description;

        // Copy options
        foreach (Option option in source.Options)
        {
            target.Options.Add(option);
        }

        // Copy arguments
        foreach (Argument argument in source.Arguments)
        {
            target.Arguments.Add(argument);
        }

        // Copy handler
        if (source.Action != null)
        {
            target.Action = source.Action;
        }

        foreach (string alias in source.Aliases)
        {
            if (!target.Aliases.Contains(alias))
            {
                target.Aliases.Add(alias);
            }
        }

        // Copy subcommands
        foreach (Command subcommand in source.Subcommands)
        {
            target.Subcommands.Add(subcommand);
        }
    }
}
