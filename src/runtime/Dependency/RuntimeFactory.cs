using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.Runtime.Dependency
{
    internal static class RuntimeFactory
    {
        public static IServiceProvider RuntimeServiceProvider(SessionState sessionState, IIoCStartup startup)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(sessionState);
            serviceCollection.AddTransient<IContextBuilder, ContextBuilder>();
            serviceCollection.AddTransient<ISessionStorageProvider, SessionStorageProvider>();
            serviceCollection.AddSingleton<ISessionState, SessionStateProxy>();
            serviceCollection.AddTransient(_ => startup);

            return serviceCollection.BuildServiceProvider();
        }

        public static IServiceProvider RuntimeServiceProvider(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDependencyBinder, DependencyBinder>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
