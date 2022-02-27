using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC
{
    public class AutomationTeardown : IoCShellBase
    {
        private IServiceProvider serviceProvider;
        private PSVariable psVariable;

        protected sealed override void BeginProcessing()
        {
            if (SessionState is not null)
            {
                psVariable = SessionState.PSVariable.Get(AutomationStartup.SERVICE_PROVIDER);

                serviceProvider = (IServiceProvider)psVariable?.Value;
            }
        }

        protected override void ProcessRecord()
        {
            WriteVerbose("Removing Dependencies");
        }

        protected sealed override void EndProcessing()
        {
            if (serviceProvider is not null)
                (serviceProvider as ServiceProvider).Dispose();
        }
    }
}