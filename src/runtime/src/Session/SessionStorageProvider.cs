// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Environment;
using Microsoft.Extensions.Hosting;
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
        environmentStorageProvider.GetEnvironmentVariable<IHost>(StorageKey)?.Services;

    public void StoreHostProvider(IHost hostProvider) =>
        environmentStorageProvider.SetEnvironmentVariable(StorageKey, hostProvider, ScopedItemOptions.ReadOnly);
}
