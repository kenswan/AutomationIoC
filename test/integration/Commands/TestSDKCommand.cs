using AutomationIoC.Integration.Services;
using AutomationIoC.Integration.Startup;
using System.Management.Automation;

namespace AutomationIoC.Commands
{
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
}
