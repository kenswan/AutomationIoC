namespace AutomationIoC.Providers
{
    public interface ISessionStorageProvider
    {
        void StoreProvider(IServiceProvider serviceProvider);

        IServiceProvider GetServiceProvider();
    }
}
