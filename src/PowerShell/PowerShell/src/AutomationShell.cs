// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using BlazorFocused.Automation.PowerShell.Session;
using BlazorFocused.Automation.Runtime;

namespace BlazorFocused.Automation.PowerShell;

/// <summary>
///
/// </summary>
/// <typeparam name="TStartup"></typeparam>
public abstract class AutomationShell<TStartup> : PSCmdlet where TStartup : IAutomationStartup, new()
{
    internal string CommandName => MyInvocation.InvocationName;

    /// <summary>
    ///
    /// </summary>
    protected override void BeginProcessing()
    {
        WriteVerbose($"Command {CommandName} Started");

        base.BeginProcessing();

        AutomationRuntime.BindServicesByAttribute<AutomationDependencyAttribute, TStartup>(
            new AutomationSessionStateProxy(SessionState), this);

        WriteVerbose($"{CommandName} Context Initialized");
    }

    /// <summary>
    ///
    /// </summary>
    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        WriteVerbose($"{CommandName} process completed");
    }

    /// <summary>
    ///
    /// </summary>
    protected override void EndProcessing()
    {
        base.EndProcessing();

        WriteVerbose($"{CommandName} operation completed");
    }
}
