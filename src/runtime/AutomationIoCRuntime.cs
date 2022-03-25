using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime
{
    public static class AutomationIoCRuntime
    {
        public static void BindContext<TAttribute, TStartup>(DependencyContext<TAttribute, TStartup> context)
            where TAttribute : Attribute
            where TStartup : IIoCStartup, new()
        {
            var sessionState = new SessionStateProxy(context.SessionState);

            IAutomationIoCBinder binder =
                new AutomationIoCBinder(RuntimeFactory.RuntimeServiceProvider(sessionState, new TStartup()));

            binder.BindContext(context);
        }

        public static void BindServiceCollection<TAttribute>(IServiceCollection serviceCollection, object instance)
            where TAttribute : Attribute
        {
            IAutomationIoCBinder binder =
                new AutomationIoCBinder(RuntimeFactory.RuntimeServiceProvider(serviceCollection));

            binder.BindServiceCollection<TAttribute>(serviceCollection, instance);
        }
    }
}
