// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Tools.Test.TestBed.Services;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Startup;
using System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools.Test.TestBed.Commands;

[Cmdlet(VerbsDiagnostic.Test, "Dependency")]
public class TestDependencyCommand : AutomationShell<TestStartup>
{
    [Parameter(Mandatory = true)]
    public int Times;

    [AutomationDependency]
    protected readonly ITestService testService;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        for (int i = 0; i < Times; i++)
        {
            testService.CallTestMethod();
        }

        WriteObject(testService.CallCount);
    }
}
