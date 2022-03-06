using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Context
{
    public interface IAutomationContext
    {
        void GenerateServices(IServiceCollection serviceCollection);

        object GetDependency(Type injectedType);
    }
}
