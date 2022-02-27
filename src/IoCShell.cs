using System.Management.Automation;

namespace AutomationIoC
{
    public abstract class IoCShell : IoCShellBase
    {
        private IServiceProvider serviceProvider;

        public IoCShell()
        { }

        public IoCShell(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected sealed override void BeginProcessing()
        {
            base.BeginProcessing();

            WriteVerbose("Starting Command");

            if (serviceProvider is null)
            {
                PSVariable psVariable = SessionState.PSVariable.Get(AutomationStartup.SERVICE_PROVIDER);

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

        protected sealed override void ProcessRecord()
        {
            base.ProcessRecord();

            ExecuteCmdlet(serviceProvider);
        }

        protected sealed override void EndProcessing()
        {
            base.EndProcessing();

            WriteVerbose("Command Complete");
        }
    }
}
