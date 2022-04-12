namespace AutomationIoC.Runtime.Session;

internal interface ISessionStorageProvider
{
    IServiceProvider GetCurrentServiceProvider();

    void StoreServiceProvider(IServiceProvider serviceProvider);
}
