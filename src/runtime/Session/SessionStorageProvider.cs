using AutomationIoC.Runtime.Environment;
using System.Management.Automation;

namespace AutomationIoC.Runtime.Session;

internal class SessionStorageProvider : ISessionStorageProvider
{
    private readonly IEnvironmentStorageProvider environmentStorageProvider;
    private readonly IIoCStartup startup;

    private string StorageKey => startup.GetType().Name;

    public SessionStorageProvider(IEnvironmentStorageProvider environmentStorageProvider, IIoCStartup startup)
    {
        this.environmentStorageProvider = environmentStorageProvider;
        this.startup = startup;
    }

    public IServiceProvider GetCurrentServiceProvider() =>
        environmentStorageProvider.GetEnvironmentVariable<IServiceProvider>(StorageKey);

    public void StoreServiceProvider(IServiceProvider serviceProvider) =>
        environmentStorageProvider.SetEnvironmentVariable(StorageKey, serviceProvider, ScopedItemOptions.ReadOnly);
}
