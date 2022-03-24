using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AutomationIoC.Runtime
{
    public static class AutomationIoCRuntime
    {
        public static void BindContext<TAttribute, TStartup>(DependencyContext<TAttribute, TStartup> context)
            where TAttribute : Attribute
            where TStartup : IIoCStartup, new()
        {
            IAutomationIoCBinder binder =
                new AutomationIoCBinder(RuntimeFactory.RuntimeServiceProvider(context.SessionState, new TStartup()));

            binder.BindContext(context);
        }

        public static void BindServiceCollection<TAttribute>(IServiceCollection serviceCollection, object instance)
            where TAttribute : Attribute
        {
            IAutomationIoCBinder binder = new AutomationIoCBinder(RuntimeFactory.RuntimeServiceProvider(serviceCollection));

            binder.BindServiceCollection<TAttribute>(serviceCollection, instance);
        }
    }
}
