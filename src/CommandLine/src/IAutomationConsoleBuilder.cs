// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Automation.CommandLine;

/// <summary>
///
/// </summary>
public interface IAutomationConsoleBuilder
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="commandPath"></param>
    /// <returns></returns>
    IAutomationConsoleBuilder AddCommand<T>(params string[] commandPath) where T : IConsoleCommand, new();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IAutomationConsole Build();
}
