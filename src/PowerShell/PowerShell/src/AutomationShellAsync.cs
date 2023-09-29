// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.Runtime;

namespace BlazorFocused.Automation.PowerShell;

/// <summary>
///
/// </summary>
/// <typeparam name="TStartup"></typeparam>
public abstract class AutomationShellAsync<TStartup> : AutomationShell<TStartup>
    where TStartup : IAutomationStartup, new()
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected abstract Task ProcessRecordAsync();

    /// <summary>
    ///
    /// </summary>
    protected sealed override void ProcessRecord() => ProcessRecordAsync().Wait();
}
