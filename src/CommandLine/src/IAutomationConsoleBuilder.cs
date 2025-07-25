// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.CommandLine;

/// <summary>
/// Automation command builder used to register commands
/// </summary>
public interface IAutomationConsoleBuilder
{
    /// <summary>
    /// Add command to registered command path/pipeline
    /// </summary>
    /// <typeparam name="T">Type of command implementation</typeparam>
    /// <param name="commandPath">Path to register command (command and subcommand path)</param>
    /// <returns>Builder pattern object used for registration continuation</returns>
    IAutomationConsoleBuilder AddCommand<T>(params string[] commandPath) where T : IConsoleCommand, new();

    /// <summary>
    /// Finalize command registration and build automation console
    /// </summary>
    /// <returns>Automation console that will run registered process</returns>
    IAutomationConsole Build();
}
