// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using AutomationIoC.PowerShell.Session;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Services;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Startup;
using AutomationIoC.Runtime;

namespace AutomationIoC.PowerShell.Tools.Test.TestBed.Commands;

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
