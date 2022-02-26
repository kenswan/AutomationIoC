using System.Management.Automation;
using PowerShellFocused.SDK;

namespace PowerShellFocused
{
    [Cmdlet(VerbsCommon.Get, "Test")]
    public class TestCmdlet : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            var testService = new TestService();

            testService.CallTestMethod();

            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
