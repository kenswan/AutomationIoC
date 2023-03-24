// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Integration.Attributes;
using AutomationIoC.PSCmdlets.Integration.Services;
using AutomationIoC.PSCmdlets.Integration.Startup;
using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC.PSCmdlets.Integration.Commands;

[Cmdlet(VerbsDiagnostic.Test, "Context")]
public class TestContextCommand : PSCmdlet
{
    [TestTools]
    protected readonly ITestService testService;

    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        var dependencyContext = new DependencyContext<TestToolsAttribute, TestStartup>
        {
            Instance = this,
            SessionState = SessionState
        };

        AutomationIoCRuntime.BindContext(dependencyContext);

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
