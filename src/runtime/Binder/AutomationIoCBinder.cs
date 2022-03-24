using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Binder
{
    internal class AutomationIoCBinder : IAutomationIoCBinder
    {
        private readonly IServiceProvider serviceProvider;

        public AutomationIoCBinder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void BindContext<TAttribute, TStartup>(DependencyContext<TAttribute, TStartup> context)
            where TAttribute : Attribute
            where TStartup : IIoCStartup, new()
        {
            using var scope = serviceProvider.CreateScope();

            var contextBuilder = scope.ServiceProvider.GetRequiredService<IContextBuilder>();

            if (!contextBuilder.IsInitialized)
                contextBuilder.BuildServices();

            contextBuilder.InitializeCurrentInstance<TAttribute>(context.Instance);
        }

        public void BindServiceCollection<TAttribute>(IServiceCollection serviceCollection, object instance) where TAttribute : Attribute
        {
            using var scope = serviceProvider.CreateScope();

            var dependencyBinder = scope.ServiceProvider.GetRequiredService<IDependencyBinder>();

            dependencyBinder.LoadFieldsByAttribute<TAttribute>(instance);
            dependencyBinder.LoadPropertiesByAttribute<TAttribute>(instance);
        }
    }
}
