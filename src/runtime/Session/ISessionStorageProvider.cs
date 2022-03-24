namespace AutomationIoC.Runtime.Session
{
    public interface ISessionStorageProvider
    {
        IServiceProvider GetCurrentServiceProvider();

        void StoreServiceProvider(IServiceProvider serviceProvider);
    }
}
