using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace PowerShellFocused
{
    public abstract class FocusedCmdlet : PSCmdlet
    {
        private IServiceProvider serviceProvider;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            PSVariable psVariable = SessionState.PSVariable.Get(FocusedStartup.SERVICE_PROVIDER);

            if (psVariable is not null)
            {
                serviceProvider = (IServiceProvider)psVariable;
            }
            else
            {
                throw new ArgumentNullException(
                    nameof(serviceProvider),
                    "Service provider is missing. Ensure Service Focused Startup was called.");
            }
        }

        protected abstract void ExecuteCmdlet(IServiceProvider serviceProvider);

        protected override void ProcessRecord()
        {

            ExecuteCmdlet(serviceProvider);
        }

        protected override void EndProcessing()
        {
            if (serviceProvider is not null)
            {
                (serviceProvider as ServiceProvider).Dispose();
            }
        }
    }
}
