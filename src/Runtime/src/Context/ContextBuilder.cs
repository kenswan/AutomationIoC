// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorFocused.Automation.Runtime.Dependency;

namespace BlazorFocused.Automation.Runtime.Context;

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

        contextKey = startup.GetType().Name;
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

    public IServiceProvider BuildServices(IServiceCollection serviceCollection)
    {
        IHost host = DependencyFactory.GenerateHost(
            startup.Configure,
            (hostBuilderContext, services) =>
            {
                foreach (ServiceDescriptor service in serviceCollection)
                {
                    services.Add(service);
                }

                startup.ConfigureServices(hostBuilderContext, services);
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
