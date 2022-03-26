using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Models;
using AutomationIoC.Runtime.Session;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AutomationIoC.Runtime.Dependency
{
    public class RuntimeFactoryTests
    {
        [Fact]
        public void ShouldBuildRuntimeProviderFromSession()
        {
            var sessionState = new SessionStateProxy(null);
            var startup = new TestRuntimeStartup();

            var actualServiceProvider = RuntimeFactory.RuntimeServiceProvider(sessionState, startup);

            var providerSessionState = actualServiceProvider.GetRequiredService<ISessionState>();
            var providerStartup = actualServiceProvider.GetRequiredService<IIoCStartup>();

            providerSessionState.Should().BeEquivalentTo(sessionState);
            providerStartup.Should().BeEquivalentTo(startup);

            Assert.NotNull(actualServiceProvider.GetService<IContextBuilder>());
            Assert.NotNull(actualServiceProvider.GetService<ISessionStorageProvider>());
            Assert.NotNull(actualServiceProvider.GetService<ISessionState>());
        }
    }
}
