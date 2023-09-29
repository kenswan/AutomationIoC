// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Automation.CommandLine;

/// <summary>
///
/// </summary>
public interface IAutomationConsole
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    int Run();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<int> RunAsync();
}
