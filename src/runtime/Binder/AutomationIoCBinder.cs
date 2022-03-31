using AutomationIoC.Runtime.Context;

namespace AutomationIoC.Runtime.Binder
{
    internal class AutomationIoCBinder : IAutomationIoCBinder
    {
        private readonly IContextBuilder contextBuilder;

        public AutomationIoCBinder(IContextBuilder contextBuilder)
        {
            this.contextBuilder = contextBuilder;
        }

        public void BindContext<TAttribute>(object instance)
            where TAttribute : Attribute
        {
            if (!contextBuilder.IsInitialized)
                contextBuilder.BuildServices();

            contextBuilder.InitializeCurrentInstance<TAttribute>(instance);
        }
    }
}
