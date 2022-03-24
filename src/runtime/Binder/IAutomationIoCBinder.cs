using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Binder
{
    internal interface IAutomationIoCBinder
    {
        void BindContext<TAttribute, TStartup>(DependencyContext<TAttribute, TStartup> context)
            where TAttribute : Attribute
            where TStartup : IIoCStartup, new();

        void BindServiceCollection<TAttribute>(IServiceCollection serviceCollection, object instance)
            where TAttribute : Attribute;
    }
}
