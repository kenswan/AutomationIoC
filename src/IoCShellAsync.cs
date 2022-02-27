namespace AutomationIoC
{
    public abstract class IoCShellAsync : IoCShell
    {
        protected abstract Task ExecuteCmdletAsync(IServiceProvider serviceProvider);

        protected sealed override void ExecuteCmdlet(IServiceProvider serviceProvider)
        {
            ExecuteCmdletAsync(serviceProvider).Wait();
        }
    }
}
