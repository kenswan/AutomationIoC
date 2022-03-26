using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Binder
{
    internal interface IAutomationIoCBinder
    {
        void BindContext<TAttribute>(object instance) where TAttribute : Attribute;

        void ImportServices(IServiceCollection serviceCollection);
    }
}
