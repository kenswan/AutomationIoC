using AutomationIoC.Runtime;
using AutomationIoC.Tools.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Tools
{
    public class TestAutomationContext
    {
        public static TestAutomationShell<TShell, TStartup> CreateInstance<TShell, TStartup>(Action<IServiceCollection> serviceAction)
            where TShell : IoCShell<TStartup>, new()
            where TStartup : IIoCStartup, new()
        {
            var shell = new TShell
            {
                CommandRuntime = new MockCommandRuntime(),
                InitializeContext = false
            };

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceAction(serviceCollection);

            AutomationIoCRuntime.BindServiceCollection<AutomationDependencyAttribute>(serviceCollection, shell);

            return new TestAutomationShell<TShell, TStartup>(shell);
        }
    }
}
