using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Dependency
{
    internal static class RuntimeFactory
    {
        public static IServiceProvider RuntimeServiceProvider(SessionStateProxy sessionState, IIoCStartup startup)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IContextBuilder, ContextBuilder>();
            serviceCollection.AddTransient<ISessionStorageProvider, SessionStorageProvider>();
            serviceCollection.AddSingleton<ISessionState>(_ => sessionState);
            serviceCollection.AddTransient(_ => startup);

            return serviceCollection.BuildServiceProvider();
        }

        public static IServiceCollection RuntimeDependencyCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            return serviceCollection.AddTransient<IDependencyBinder, DependencyBinder>();
        }
    }
}
