// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Integration.Services;
using AutomationIoC.PSCmdlets.Integration.Startup;
using System.Management.Automation;

namespace AutomationIoC.PSCmdlets.Integration.Commands;

[Cmdlet(VerbsCommon.Get, "TestSDKCommand")]
public class TestSDKCommand : IoCShell<TestStartup>
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
