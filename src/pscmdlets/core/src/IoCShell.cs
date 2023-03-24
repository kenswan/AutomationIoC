// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoc.PSCmdlets;

public abstract class IoCShell<TStartup> : PSCmdlet where TStartup : IIoCStartup, new()
{
    internal string CommandName => MyInvocation.InvocationName;

    protected override void BeginProcessing()
    {
        WriteVerbose($"Command {CommandName} Started");

        base.BeginProcessing();

        var dependencyContext = new DependencyContext<AutomationDependencyAttribute, TStartup>
        {
            Instance = this,
            SessionState = SessionState
        };

        AutomationIoCRuntime.BindContext(dependencyContext);

        WriteVerbose($"{CommandName} Context Initialized");
    }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        WriteVerbose($"{CommandName} process completed");
    }

    protected override void EndProcessing()
    {
        base.EndProcessing();

        WriteVerbose($"{CommandName} operation completed");
    }
}
