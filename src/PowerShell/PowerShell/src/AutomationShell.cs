// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Session;
using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC.PowerShell;

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

        ISessionStorage sessionStorage = new AutomationSessionStateProxy(SessionState);

        if (AutomationRuntime.HasRegisteredServiceProvider<TStartup>(sessionStorage, out IServiceProvider serviceProvider))
        {
            AutomationRuntime.BindServicesByAttribute<AutomationDependencyAttribute>(serviceProvider, this);
        }
        else
        {
            AutomationRuntime.BindServicesByAttribute<AutomationDependencyAttribute, TStartup>(sessionStorage, this);
        }

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
