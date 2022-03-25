using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Context
{
    internal interface IContextBuilder
    {
        bool IsInitialized { get; }

        void BuildServices();

        void InitializeCurrentInstance<TAttribute>(object instance) where TAttribute : Attribute;
    }
}
