using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime.Session;

internal interface ISessionStorageProvider
{
    IServiceProvider GetCurrentServiceProvider();

    void StoreHostProvider(IHost host);
}
