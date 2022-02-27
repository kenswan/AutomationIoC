using System.Management.Automation;
using Microsoft.Extensions.DependencyInjection;

namespace PowerShellFocused.Test
{
    [Cmdlet(VerbsCommon.Get, "Test")]
    public class TestCmdlet : FocusedCmdlet
    {
        public void RunInstance()
        {
            this.BeginProcessing();
            this.ProcessRecord();
            this.EndProcessing();
        }

        protected override void ExecuteCmdlet(IServiceProvider serviceProvider)
        {
            var testService = serviceProvider.GetRequiredService<TestService>();

            WriteVerbose($"From Test Service: {testService.CallTestMethod()}");
        }
    }
}
