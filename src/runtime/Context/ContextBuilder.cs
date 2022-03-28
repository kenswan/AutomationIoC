using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutomationIoC.Runtime.Context
{
    internal class ContextBuilder : IContextBuilder
    {
        private readonly IIoCStartup startup;
        private readonly ISessionStorageProvider sessionStorageProvider;

        public ContextBuilder(IIoCStartup startup, ISessionStorageProvider sessionStorageProvider)
        {
            this.startup = startup;
            this.sessionStorageProvider = sessionStorageProvider;
        }

        public bool IsInitialized => sessionStorageProvider.GetCurrentServiceProvider() is not null;

        public void BuildServices()
        {
            var configurationBuilder = new ConfigurationBuilder();

            startup.Configure(configurationBuilder);

            startup.Configuration = configurationBuilder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(startup.Configuration);

            serviceCollection.AddLogging(builder => builder.AddConsole());

            startup.ConfigureServices(serviceCollection);

            RuntimeFactory.AddClientRuntime(serviceCollection);

            sessionStorageProvider.StoreServiceProvider(serviceCollection.BuildServiceProvider());
        }

        public void BuildServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(builder => builder.AddConsole());

            RuntimeFactory.AddClientRuntime(serviceCollection);

            sessionStorageProvider.StoreServiceProvider(serviceCollection.BuildServiceProvider());
        }

        public void InitializeCurrentInstance<TAttribute>(object instance) where TAttribute : Attribute
        {
            var serviceProvider = sessionStorageProvider.GetCurrentServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dependencyBinder = scope.ServiceProvider.GetRequiredService<IDependencyBinder>();

            dependencyBinder.LoadFieldsByAttribute<TAttribute>(instance);

            dependencyBinder.LoadPropertiesByAttribute<TAttribute>(instance);
        }
    }
}
