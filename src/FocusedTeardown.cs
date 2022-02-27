using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace PowerShellFocused
{
    public class FocusedTeardown : FocusedCmdletBase
    {
        private IServiceProvider serviceProvider;
        private PSVariable psVariable;

        protected override void BeginProcessing()
        {
            if (SessionState is not null)
            {
                psVariable = SessionState.PSVariable.Get(FocusedStartup.SERVICE_PROVIDER);

                serviceProvider = (IServiceProvider)psVariable?.Value;
            }
        }

        protected override void ProcessRecord()
        {
            WriteVerbose("Removing Dependencies");
        }

        protected override void EndProcessing()
        {
            if (serviceProvider is not null)
                (serviceProvider as ServiceProvider).Dispose();

            // TODO: Force Removal since it is readonly
            /* if (psVariable is not null)
                SessionState.PSVariable.Remove(psVariable); */
        }
    }
}