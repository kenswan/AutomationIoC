using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Binder
{
    internal class AutomationIoCBinder : IAutomationIoCBinder
    {
        private readonly IServiceProvider serviceProvider;

        public AutomationIoCBinder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void BindContext<TAttribute>(object instance)
            where TAttribute : Attribute
        {
            using var scope = serviceProvider.CreateScope();

            var contextBuilder = scope.ServiceProvider.GetRequiredService<IContextBuilder>();

            if (!contextBuilder.IsInitialized)
                contextBuilder.BuildServices();

            contextBuilder.InitializeCurrentInstance<TAttribute>(instance);
        }

        public void ImportServices(IServiceCollection serviceCollection)
        {
            using var scope = serviceProvider.CreateScope();

            var sessionStorageProvider = scope.ServiceProvider.GetRequiredService<ISessionStorageProvider>();

            sessionStorageProvider.StoreServiceProvider(serviceCollection.BuildServiceProvider());
        }
    }
}
