namespace PowerShellFocused
{
    public abstract class FocusedAsyncCmdlet : FocusedCmdlet
    {
        protected abstract Task ExecuteCmdletAsync(IServiceProvider serviceProvider);

        protected override void ExecuteCmdlet(IServiceProvider serviceProvider)
        {
            ExecuteCmdletAsync(serviceProvider).Wait();
        }
    }
}
