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
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(context.SessionState);
            serviceCollection.AddTransient<IContextBuilder, ContextBuilder>();
            serviceCollection.AddSingleton<ISessionState, InternalSessionState>();
            serviceCollection.AddTransient<IDependencyBinder, DependencyBinder>();
            serviceCollection.AddTransient<IIoCStartup>(_ => new TStartup());

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var contextBuilder = scope.ServiceProvider.GetRequiredService<IContextBuilder>();

            if(!contextBuilder.IsInitialized)
                contextBuilder.BuildServices();

            contextBuilder.InitializeCurrentInstance<TAttribute>(context.Instance);
        }

        public static void BindServiceCollection<TAttribute>(IServiceCollection serviceCollection, object instance)
            where TAttribute : Attribute
        {
            serviceCollection.TryAddTransient<IDependencyBinder, DependencyBinder>();

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var dependencyBinder = scope.ServiceProvider.GetRequiredService<IDependencyBinder>();

            dependencyBinder.LoadFieldsByAttribute<TAttribute>(instance);
            dependencyBinder.LoadPropertiesByAttribute<TAttribute>(instance);
        }
    }
}
