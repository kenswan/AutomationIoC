﻿// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime.Context;

internal class ContextBuilder : IContextBuilder
{
    private readonly IAutomationStartup startup;
    private readonly ISessionStorage sessionStorage;
    private readonly string contextKey;

    private bool isInitializedInScope;

    public ContextBuilder(
        IAutomationStartup startup,
        ISessionStorage sessionState)
    {
        this.startup = startup;
        this.sessionStorage = sessionState;

        contextKey = startup.GetType().FullName;
        isInitializedInScope = false;
    }

    public bool IsInitialized => isInitializedInScope || GetCurrentHost() is not null;

    public IServiceProvider BuildServices()
    {
        IHost host = DependencyFactory.GenerateHost(
            startup.Configure,
            startup.ConfigureServices,
            startup.GenerateParameters(),
            startup.GenerateParameterConfigurationMapping());

        SetHostStorage(host);

        return host.Services;
    }

    public IServiceProvider BuildServices(Action<IServiceCollection> servicesOverride)
    {
        IHost host = DependencyFactory.GenerateHost(
            startup.Configure,
            (hostBuilderContext, services) =>
            {
                startup.ConfigureServices(hostBuilderContext, services);
                servicesOverride(services);
            },
            startup.GenerateParameters(),
            startup.GenerateParameterConfigurationMapping());

        SetHostStorage(host);

        return host.Services;
    }

    public IServiceProvider GetContextServiceProvider() =>
        GetCurrentHost()?.Services ?? BuildServices();

    private IHost GetCurrentHost() => sessionStorage.GetValue<IHost>(contextKey);

    private void SetHostStorage(IHost host)
    {
        sessionStorage.SetValue(contextKey, host);

        isInitializedInScope = true;
    }
}
