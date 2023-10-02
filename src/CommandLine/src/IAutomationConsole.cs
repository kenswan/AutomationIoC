// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Automation.CommandLine;

/// <summary>
/// Console automation class used to register commands and run them based on command line arguments
/// </summary>
public interface IAutomationConsole
{
    /// <summary>
    /// Run registered command synchronously based on command line arguments
    /// </summary>
    /// <returns>Result status code for process</returns>
    int Run();

    /// <summary>
    /// Run registered command asynchronously based on command line arguments
    /// </summary>
    /// <returns>Result status code for process</returns>
    Task<int> RunAsync();
}
