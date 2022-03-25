using AutomationIoC.Runtime.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Xunit;

namespace AutomationIoC.Runtime.Session
{
    public class SessionProviderTests
    {
        [Fact]
        public void ShouldGetCurrentServiceProviderFromSession()
        {
            var startup = new TestRuntimeStartup();
            var sessionStateMock = new Mock<ISessionState>();
            var serviceProvider = new ServiceCollection().BuildServiceProvider();

            PSVariable serviceVariable =
                    new(startup.GetType().Name, serviceProvider, ScopedItemOptions.ReadOnly);

            var runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
            runspace.Open();

            var psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

            psVariableIntrinsics.Set(serviceVariable);

            sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);

            var sessionStorageProvider = new SessionStorageProvider(sessionStateMock.Object, startup);

            var actualServiceProvider = sessionStorageProvider.GetCurrentServiceProvider();

            actualServiceProvider.Should().BeEquivalentTo(serviceProvider);
        }

        [Fact]
        public void ShouldReturnNullIfServiceProviderDoesNotExist()
        {
            var startup = new TestRuntimeStartup();
            var sessionStateMock = new Mock<ISessionState>();
            var runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
            runspace.Open();

            var psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

            sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);

            var sessionStorageProvider = new SessionStorageProvider(sessionStateMock.Object, startup);

            var actualServiceProvider = sessionStorageProvider.GetCurrentServiceProvider();

            actualServiceProvider.Should().BeNull();
        }

        [Fact]
        public void ShouldStoreServiceProvider()
        {
            var startup = new TestRuntimeStartup();
            var sessionStateMock = new Mock<ISessionState>();
            var serviceProvider = new ServiceCollection().BuildServiceProvider();

            var runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
            runspace.Open();

            var psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

            sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);

            var sessionStorageProvider = new SessionStorageProvider(sessionStateMock.Object, startup);

            sessionStorageProvider.StoreServiceProvider(serviceProvider);

            var storedServiceProvider = sessionStorageProvider.GetCurrentServiceProvider();

            storedServiceProvider.Should().BeEquivalentTo(serviceProvider);
        }
    }
}
