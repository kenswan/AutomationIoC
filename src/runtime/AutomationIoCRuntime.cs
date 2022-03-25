using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Dependency;
using Microsoft.Extensions.DependencyInjection;

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
            IAutomationIoCBinder binder =
                new AutomationIoCBinder(RuntimeFactory.RuntimeServiceProvider(serviceCollection));

            binder.BindServiceCollection<TAttribute>(serviceCollection, instance);
        }
    }
}
