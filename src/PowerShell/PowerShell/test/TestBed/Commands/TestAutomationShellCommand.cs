// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Test.Services;
using AutomationIoC.PowerShell.Test.TestBed.Startup;
using System.Management.Automation;

namespace AutomationIoC.PowerShell.Test.TestBed.Commands;

[Cmdlet(VerbsCommon.Get, "TestSDKCommand")]
public class TestAutomationShellCommand : AutomationShell<TestStartup>
{
    [AutomationDependency]
    protected readonly ITestService testService;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        testService.CallTestMethod();
        testService.CallTestMethod();
        testService.CallTestMethod();

        WriteObject(testService.CallCount);
    }
}
