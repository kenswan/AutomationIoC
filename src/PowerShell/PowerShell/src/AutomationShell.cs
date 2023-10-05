// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Session;
using BlazorFocused.Automation.Runtime;
using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell;

/// <summary>
/// Standard automation class used to run synchronous PowerShell automation commands
/// </summary>
/// <typeparam name="TStartup">Type of automation startup services class</typeparam>
public abstract class AutomationShell<TStartup> : PSCmdlet where TStartup : IAutomationStartup, new()
{
    internal string CommandName => MyInvocation.InvocationName;

    /// <summary>
    /// Pre-process preparation for command execution
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
    /// Main command execution process
    /// </summary>
    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        WriteVerbose($"{CommandName} process completed");
    }

    /// <summary>
    /// Post-process cleanup after command execution
    /// </summary>
    protected override void EndProcessing()
    {
        base.EndProcessing();

        WriteVerbose($"{CommandName} operation completed");
    }
}
