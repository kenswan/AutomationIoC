using Microsoft.Extensions.DependencyInjection;
using PowerShellFocused.Services;
using System.Management.Automation;

namespace PowerShellFocused.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Test")]
    public class TestCmdlet : FocusedCmdlet
    {
        public TestCmdlet(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override void ExecuteCmdlet(IServiceProvider serviceProvider)
        {
            var testService = serviceProvider.GetRequiredService<TestService>();

            WriteVerbose($"From Test Service: {testService.CallTestMethod()}");
        }
    }
}
