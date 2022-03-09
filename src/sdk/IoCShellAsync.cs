namespace AutomationIoC
{
    public abstract class IoCShellAsync : IoCShell
    {
        protected abstract Task ExecuteCmdletAsync();

        protected sealed override void ExecuteCmdlet()
        {
            ExecuteCmdletAsync().Wait();
        }
    }
}
