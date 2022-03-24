using AutomationIoC.Runtime;
using AutomationIoC.Runtime.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Tools
{
    internal class TestAutomationContext
    {
        public TestAutomationShell<TShell, TStartup> CreateInstance<TShell, TStartup>(params object[] dependencies)
            where TShell : IoCShell<TStartup>, new()
            where TStartup : IIoCStartup, new()
        {
            var shell = new TShell();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IDependencyBinder, DependencyBinder>();

            foreach(var dependency in dependencies)
                serviceCollection.AddSingleton(dependency.GetType(), dependency);

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            var binder = serviceProvider.GetRequiredService<IDependencyBinder>();

            binder.LoadFieldsByAttribute<AutomationDependencyAttribute>(shell);
            binder.LoadPropertiesByAttribute<AutomationDependencyAttribute>(shell);

            return new TestAutomationShell<TShell, TStartup>(shell);
        }
    }
}
