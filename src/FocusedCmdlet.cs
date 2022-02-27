using System.Management.Automation;

namespace AutomationIoC
{
    public abstract class FocusedCmdlet : FocusedCmdletBase
    {
        private IServiceProvider serviceProvider;

        public FocusedCmdlet()
        {

        }

        public FocusedCmdlet(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            WriteVerbose("Starting Command");

            if (serviceProvider is null)
            {
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
