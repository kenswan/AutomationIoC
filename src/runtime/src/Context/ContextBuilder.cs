using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AutomationIoC.Runtime.Context;

internal class ContextBuilder : IContextBuilder
{
    private readonly IAutomationEnvironment automationEnvironment;
    private readonly IIoCStartup startup;
    private readonly ISessionStorageProvider sessionStorageProvider;

    public ContextBuilder(
        IAutomationEnvironment automationEnvironment,
        IIoCStartup startup,
        ISessionStorageProvider sessionStorageProvider)
    {
        this.automationEnvironment = automationEnvironment;
        this.startup = startup;
        this.sessionStorageProvider = sessionStorageProvider;
    }

    public bool IsInitialized => sessionStorageProvider.GetCurrentServiceProvider() is not null;

    public void BuildServices()
    {
        startup.AutomationEnvironment = automationEnvironment;

        IHost hostApplication = GenerateHostBuilder()
            .ConfigureAppConfiguration(startup.Configure)
            .ConfigureServices(services =>
            {
                startup.ConfigureServices(services);

                RuntimeFactory.AddClientRuntime(services);
            })
            .Build();

        sessionStorageProvider.StoreHostProvider(hostApplication);
    }

    public void BuildServices(IServiceCollection serviceCollection)
    {
        IHost hostApplication = GenerateHostBuilder()
            .ConfigureServices(services =>
            {
                foreach (ServiceDescriptor service in serviceCollection)
                {
                    services.Add(service);
                }

                RuntimeFactory.AddClientRuntime(services);
            })
            .Build();

        sessionStorageProvider.StoreHostProvider(hostApplication);
    }

    public void InitializeCurrentInstance<TAttribute>(object instance) where TAttribute : Attribute
    {
        IServiceProvider serviceProvider = sessionStorageProvider.GetCurrentServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();
        IDependencyBinder dependencyBinder = scope.ServiceProvider.GetRequiredService<IDependencyBinder>();

        dependencyBinder.LoadFieldsByAttribute<TAttribute>(instance);

        dependencyBinder.LoadPropertiesByAttribute<TAttribute>(instance);
    }

    private static IHostBuilder GenerateHostBuilder() =>
        new HostBuilder()
        .ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.AddDebug();
            loggingBuilder.AddConsole();
        });
}
