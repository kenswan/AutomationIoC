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
        [Fact]
        public void ShouldInitializeyContext()
        {
            var instance = new TestInstance(123);
            var contextBuilderMock = new Mock<IContextBuilder>();
            contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(false);

            IServiceProvider serviceProvider =
                new ServiceCollection()
                    .AddTransient(_ => contextBuilderMock.Object)
                    .BuildServiceProvider();

            new AutomationIoCBinder(serviceProvider).BindContext<TestRuntimeAttribute>(instance);

            contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Once);

            contextBuilderMock.Verify(builder =>
                builder.InitializeCurrentInstance<TestRuntimeAttribute>(instance),
                    Times.Once);
        }

        [Fact]
        public void ShouldNotInitializeContextIfAlreadySet()
        {
            var contextBuilderMock = new Mock<IContextBuilder>();
            contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(true);

            IServiceProvider serviceProvider =
                new ServiceCollection()
                    .AddTransient(_ => contextBuilderMock.Object)
                    .BuildServiceProvider();

            var instance = new TestInstance(2);

            new AutomationIoCBinder(serviceProvider).BindContext<TestRuntimeAttribute>(instance);

            contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Never);

            contextBuilderMock.Verify(builder =>
                builder.InitializeCurrentInstance<TestRuntimeAttribute>(instance),
                    Times.Once);
        }

        [Fact]
        public void ShouldBindExistingServiceCollection()
        {
            var sessionStorageProvider = new Mock<ISessionStorageProvider>();
            var instance = new TestInstance(2);

            IServiceProvider runtimeServiceProvider =
                new ServiceCollection()
                    .AddTransient(_ => sessionStorageProvider.Object)
                    .BuildServiceProvider();

            IServiceCollection importedServiceCollection =
                new ServiceCollection()
                    .AddTransient<ITestRuntimeService, TestRuntimeService>();

            new AutomationIoCBinder(runtimeServiceProvider).ImportServices(importedServiceCollection);

            sessionStorageProvider.Verify(provider =>
                provider.StoreServiceProvider(It.Is<IServiceProvider>(provider => ServiceProviderIsConfigured(provider))),
                    Times.Once);
        }

        private bool ServiceProviderIsConfigured(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<ITestRuntimeService>() is not null;
        }
    }
}
