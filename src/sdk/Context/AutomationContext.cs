using AutomationIoC.Providers;
using AutomationIoC.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.Context
{
    internal class AutomationContext : IAutomationContext

    {
        private readonly ISessionStorageProvider sessionStorageProvider;
        private readonly IDependencyService dependencyService;

        public AutomationContext(SessionState sessionState)
        {
            sessionStorageProvider = new SessionStorageProvider(sessionState);
            dependencyService = new DependencyService(sessionStorageProvider);
        }

        public AutomationContext(ISessionStorageProvider sessionStorageProvider)
        {
            this.sessionStorageProvider = sessionStorageProvider;
            dependencyService = new DependencyService(sessionStorageProvider);
        }

        public void BuildServices(IServiceCollection serviceCollection)
        {
            sessionStorageProvider.StoreProvider(serviceCollection.BuildServiceProvider());
        }

        public void InitializeCurrentInstance(object instance)
        {
            dependencyService.LoadFieldsByAttribute<AutomationDependencyAttribute>(instance);

            dependencyService.LoadPropertiesByAttribute<AutomationDependencyAttribute>(instance);
        }
    }
}
