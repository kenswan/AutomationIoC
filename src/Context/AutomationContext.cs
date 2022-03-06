using AutomationIoC.Providers;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.Context
{
    internal class AutomationContext : IAutomationContext
    {
        private readonly ISessionStorageProvider sessionStorageProvider;

        public AutomationContext(SessionState sessionState)
        {
            sessionStorageProvider = new SessionStorageProvider(sessionState);
        }

        public AutomationContext(ISessionStorageProvider sessionStorageProvider)
        {
            this.sessionStorageProvider = sessionStorageProvider;
        }

        public void GenerateServices(IServiceCollection serviceCollection)
        {
            sessionStorageProvider.StoreProvider(serviceCollection.BuildServiceProvider());
        }

        public object GetDependency(Type injectedType)
        {
            IServiceProvider serviceProvider = sessionStorageProvider.GetServiceProvider(); ;

            return serviceProvider.GetService(injectedType);
        }
    }
}
