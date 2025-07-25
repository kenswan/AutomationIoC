// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;

namespace AutomationIoC.PowerShell;

/// <summary>
/// Standard automation class used to run asynchronous PowerShell automation commands
/// </summary>
/// <typeparam name="TStartup">Type of automation startup services class</typeparam>
public abstract class AutomationShellAsync<TStartup> : AutomationShell<TStartup>
    where TStartup : IAutomationStartup, new()
{
    /// <summary>
    /// Main command execution process
    /// </summary>
    /// <returns>Asynchronous task of main command execution process</returns>
    protected abstract Task ProcessRecordAsync();

    /// <inheritdoc />
    protected sealed override void ProcessRecord() => ProcessRecordAsync().Wait();
}
