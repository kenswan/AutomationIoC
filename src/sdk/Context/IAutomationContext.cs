using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Context
{
    public interface IAutomationContext
    {
        void BuildServices(IServiceCollection serviceCollection);

        void InitializeCurrentInstance(object instance);
    }
}
