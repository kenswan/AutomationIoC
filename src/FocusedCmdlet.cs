using System.Management.Automation;

namespace PowerShellFocused
{
    public abstract class FocusedCmdlet : PSCmdlet
    {
        private IServiceProvider serviceProvider;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            WriteVerbose("Starting Command");

            PSVariable psVariable = SessionState.PSVariable.Get(FocusedStartup.SERVICE_PROVIDER);

            if (psVariable?.Value is not null)
            {
                serviceProvider = (IServiceProvider)psVariable.Value;
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
            base.ProcessRecord();

            ExecuteCmdlet(serviceProvider);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            
            WriteVerbose("Command Complete");
        }
    }
}
