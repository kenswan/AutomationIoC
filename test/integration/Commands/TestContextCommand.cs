using AutomationIoC.Integration.Attributes;
using AutomationIoC.Integration.Services;
using AutomationIoC.Integration.Startup;
using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC.Integration.Commands;

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
