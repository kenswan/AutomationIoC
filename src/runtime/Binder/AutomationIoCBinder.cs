using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Binder
{
    internal class AutomationIoCBinder : IAutomationIoCBinder
    {
        private readonly IContextBuilder contextBuilder;
        private readonly ISessionStorageProvider sessionStorageProvider;

        public AutomationIoCBinder(
            IContextBuilder contextBuilder,
            ISessionStorageProvider sessionStorageProvider)
        {
            this.contextBuilder = contextBuilder;
            this.sessionStorageProvider = sessionStorageProvider;
        }

        public void BindContext<TAttribute>(object instance)
            where TAttribute : Attribute
        {
            if (!contextBuilder.IsInitialized)
                contextBuilder.BuildServices();

            contextBuilder.InitializeCurrentInstance<TAttribute>(instance);
        }

        public void ImportServices(IServiceCollection serviceCollection)
        {
            sessionStorageProvider.StoreServiceProvider(serviceCollection.BuildServiceProvider());
        }
    }
}
