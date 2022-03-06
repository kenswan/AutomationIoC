using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC
{
    public class AutomationTeardown : IoCShellBase
    {
        protected sealed override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            WriteVerbose("Removing Dependencies");
        }

        protected sealed override void EndProcessing()
        {
        }
    }
}