using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.Runtime
{
    public static class AutomationIoCRuntime
    {
        public static void BindContext<TAttribute, TStartup>(DependencyContext<TAttribute, TStartup> context)
            where TAttribute : Attribute
            where TStartup : IIoCStartup, new()
        {
            var sessionStateProxy = new SessionStateProxy(context.SessionState);

            IAutomationIoCBinder binder =
                new AutomationIoCBinder(RuntimeFactory.RuntimeServiceProvider(sessionStateProxy, new TStartup()));

            binder.BindContext<TAttribute>(context.Instance);
        }

        public static void ImportContext<TStartup>(SessionState sessionState, IServiceCollection serviceCollection)
            where TStartup : IIoCStartup, new()
        {
            var sessionStateProxy = new SessionStateProxy(sessionState);

            IAutomationIoCBinder binder =
                new AutomationIoCBinder(RuntimeFactory.RuntimeServiceProvider(sessionStateProxy, new TStartup()));

            binder.ImportServices(serviceCollection);
        }

        public static IServiceCollection ExportRuntimeDependencies() => RuntimeFactory.RuntimeDependencyCollection();
    }
}
