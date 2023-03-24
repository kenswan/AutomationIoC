// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime.Session;

internal interface ISessionStorageProvider
{
    IServiceProvider GetCurrentServiceProvider();

    void StoreHostProvider(IHost host);
}
