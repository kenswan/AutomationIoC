// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.CommandLine;

/// <summary>
///     Configures runtime automation commands established in <see cref="IAutomationConsoleBuilder" />.
/// </summary>
public interface IAutomationCommandInitializer
{
    /// <summary>
    ///     Configures the command line automation command with the provided
    ///     <paramref name="command" /> (<see cref="AutomationCommand" />).
    ///     This method is called during the initialization phase of the command line application to set up the
    ///     command's properties, options, and handlers.
    ///     The command is typically created by the <see cref="IAutomationConsoleBuilder" />
    ///     and passed to this method for configuration.
    ///     The command can be used to define the command's name, description, options, and handlers (actions),
    ///     allowing it to be executed as part of the command line interface.
    /// </summary>
    /// <param name="command">Automation Command being configured for use</param>
    public void Initialize(IAutomationCommand command);
}
