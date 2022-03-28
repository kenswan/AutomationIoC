using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Models;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AutomationIoC.Runtime.Binder
{
    public class AutomationIoCBinderTests
    {
        private readonly Mock<IContextBuilder> contextBuilderMock;
        private readonly Mock<ISessionStorageProvider> sessionStorageProviderMock;

        private readonly AutomationIoCBinder binder;

        public AutomationIoCBinderTests()
        {
            contextBuilderMock = new();
            sessionStorageProviderMock = new();

            binder = new(contextBuilderMock.Object, sessionStorageProviderMock.Object);
        }

        [Fact]
        public void ShouldInitializeyContext()
        {
            var instance = new TestInstance(123);

            contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(false);

            binder.BindContext<TestRuntimeAttribute>(instance);

            contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Once);

            contextBuilderMock.Verify(builder =>
                builder.InitializeCurrentInstance<TestRuntimeAttribute>(instance),
                    Times.Once);
        }

        [Fact]
        public void ShouldNotInitializeContextIfAlreadySet()
        {
            var instance = new TestInstance(2);

            contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(true);

            binder.BindContext<TestRuntimeAttribute>(instance);

            contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Never);

            contextBuilderMock.Verify(builder =>
                builder.InitializeCurrentInstance<TestRuntimeAttribute>(instance),
                    Times.Once);
        }

        [Fact]
        public void ShouldBindExistingServiceCollection()
        {
            IServiceCollection importedServiceCollection =
                new ServiceCollection()
                    .AddTransient<ITestRuntimeService, TestRuntimeService>();

            binder.ImportServices(importedServiceCollection);

            sessionStorageProviderMock.Verify(provider =>
                provider.StoreServiceProvider(It.Is<IServiceProvider>(provider => ServiceProviderIsConfigured(provider))),
                    Times.Once);
        }

        private static bool ServiceProviderIsConfigured(IServiceProvider serviceProvider) =>
            serviceProvider.GetService<ITestRuntimeService>() is not null;
    }
}
