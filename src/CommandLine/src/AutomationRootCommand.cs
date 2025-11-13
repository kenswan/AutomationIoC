// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using System.CommandLine;

namespace AutomationIoC.CommandLine;

/// <summary>
///     Extends root command class with built-in dependency injection support for actions.
///     All sub-commands should inherit from AutomationCommand to leverage dependency injection.
/// </summary>
public class AutomationRootCommand(
    IAutomationContext automationContext,
    string description = "") : RootCommand(description), IAutomationCommand
{
    /// <inheritdoc />
    public IAutomationContext Context => automationContext;

    /// <inheritdoc />
    public void SetAction(Action<ParseResult, IAutomationContext> action) =>
        SetAction(parsedResult => action(parsedResult, automationContext));

    /// <inheritdoc />
    public void SetAction(Func<ParseResult, IAutomationContext, CancellationToken, Task> action) =>
        SetAction(async (parsedResult, cancellationToken) =>
        {
            await action(parsedResult, automationContext, cancellationToken).ConfigureAwait(false);
        });
}
