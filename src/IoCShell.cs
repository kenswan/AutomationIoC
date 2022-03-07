namespace AutomationIoC
{
    public abstract class IoCShell : IoCShellBase
    {
        protected abstract void ExecuteCmdlet();

        protected sealed override void ProcessRecord()
        {
            base.ProcessRecord();

            ExecuteCmdlet();
        }

        protected sealed override void EndProcessing()
        {
            base.EndProcessing();

            WriteVerbose("Command Complete");
        }
    }
}
