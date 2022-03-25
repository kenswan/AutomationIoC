using AutomationIoC.Integration.Services;
using System.Management.Automation;

namespace AutomationIoC.Commands
{
    [Cmdlet(VerbsCommon.Get, "Test")]
    public class TestModule : IoCShell<TestStartup>
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
