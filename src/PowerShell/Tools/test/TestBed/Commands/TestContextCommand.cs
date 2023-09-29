// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using BlazorFocused.Automation.PowerShell.Session;
using BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Services;
using BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Startup;
using BlazorFocused.Automation.Runtime;

namespace BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Commands;

[Cmdlet(VerbsDiagnostic.Test, "Context")]
public class TestContextCommand : PSCmdlet
{
    [AutomationDependency]
    protected readonly ITestService testService;

    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        AutomationRuntime
            .BindServicesByAttribute<AutomationDependencyAttribute, TestStartup>(
                new AutomationSessionStateProxy(SessionState), this);

    }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        testService.CallTestMethod();
        testService.CallTestMethod();
        testService.CallTestMethod();

        WriteObject(testService.CallCount);
    }
}
