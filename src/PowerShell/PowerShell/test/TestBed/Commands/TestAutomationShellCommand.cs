// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Test.Services;
using BlazorFocused.Automation.PowerShell.Test.TestBed.Startup;
using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell.Test.TestBed.Commands;

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
