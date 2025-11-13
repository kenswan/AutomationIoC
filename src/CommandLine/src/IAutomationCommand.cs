// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using System.CommandLine;

namespace AutomationIoC.CommandLine;

/// <summary>
///     Extends command classes with built-in dependency injection support for actions.
/// </summary>
public interface IAutomationCommand
{
    /// <summary>
    ///     Services and configurations for dependency injection.
    /// </summary>
    IAutomationContext Context { get; }

    /// <summary>
    ///     Adds a <see cref="Argument" /> to the command.
    /// </summary>
    /// <param name="argument">The option to add to the command.</param>
    public void Add(Argument argument);

    /// <summary>
    ///     Adds a <see cref="Option" /> to the command.
    /// </summary>
    /// <param name="option">The option to add to the command.</param>
    public void Add(Option option);

    /// <summary>
    ///     Set the action to be executed when the command is invoked (synchronous).
    ///     The action receives the parsed result and the automation context, allowing for dependency injection.
    /// </summary>
    /// <param name="action">Dependency Injection Action for synchronous command invocation</param>
    void SetAction(Action<ParseResult, IAutomationContext> action);

    /// <summary>
    ///     Set the action to be executed when the command is invoked (asynchronous).
    ///     The action receives the parsed result and the automation context, allowing for dependency injection.
    /// </summary>
    /// <param name="action">Dependency Injection Action for asynchronous command invocation</param>
    void SetAction(Func<ParseResult, IAutomationContext, CancellationToken, Task> action);
}
