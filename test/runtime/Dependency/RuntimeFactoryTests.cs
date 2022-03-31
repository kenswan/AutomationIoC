using AutomationIoC.Runtime.Binder;
using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Environment;
using AutomationIoC.Runtime.Models;
using AutomationIoC.Runtime.Session;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Management.Automation;
using Xunit;
using Runspace = System.Management.Automation.Runspaces;

namespace AutomationIoC.Runtime.Dependency
{
    public class RuntimeFactoryTests
    {
        [Fact]
        public void ShouldBuildRuntimeProviderFromSession()
        {
            var sessionState = new SessionStateProxy(null as SessionState);
            var startup = new TestRuntimeStartup();

            var actualServiceProvider = RuntimeFactory.RuntimeServiceProvider(sessionState, startup);

            var providerSessionState = actualServiceProvider.GetRequiredService<ISessionState>();
            var providerStartup = actualServiceProvider.GetRequiredService<IIoCStartup>();

            providerSessionState.Should().BeEquivalentTo(sessionState);
            providerStartup.Should().BeEquivalentTo(startup);

            Assert.NotNull(actualServiceProvider.GetService<IAutomationIoCBinder>());
            Assert.NotNull(actualServiceProvider.GetService<IAutomationEnvironment>());
            Assert.NotNull(actualServiceProvider.GetService<IContextBuilder>());
            Assert.NotNull(actualServiceProvider.GetService<IEnvironmentStorageProvider>());
            Assert.NotNull(actualServiceProvider.GetService<ISessionStorageProvider>());
            Assert.NotNull(actualServiceProvider.GetService<ISessionState>());
        }

        [Fact]
        public void ShouldBuildRuntimeProviderFromSessionStatProxy()
        {
            var sessionState = new SessionStateProxy(null as Runspace.SessionStateProxy);
            var startup = new TestRuntimeStartup();

            var actualServiceProvider = RuntimeFactory.RuntimeServiceProvider(sessionState);

            var providerSessionState = actualServiceProvider.GetRequiredService<ISessionState>();

            providerSessionState.Should().BeEquivalentTo(sessionState);

            Assert.NotNull(actualServiceProvider.GetService<IAutomationEnvironment>());
            Assert.NotNull(actualServiceProvider.GetService<IEnvironmentStorageProvider>());
            Assert.NotNull(actualServiceProvider.GetService<ISessionState>());
        }

        [Fact]
        public void ShouldAddServicesForClientRuntime()
        {
            var serviceCollection = new ServiceCollection();

            RuntimeFactory.AddClientRuntime(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            Assert.NotNull(serviceProvider.GetService<IDependencyBinder>());
            Assert.NotNull(serviceProvider.GetService<ILogger<RuntimeFactoryTests>>());
        }
    }
}
