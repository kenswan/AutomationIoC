using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Models;
using AutomationIoC.Runtime.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AutomationIoC.Runtime.Context
{
    public class ContextBuilderTests
    {
        [Fact]
        public void ShouldBuildServices()
        {
            var startup = new TestRuntimeStartup();
            var sessionStorageProviderMock = new Mock<ISessionStorageProvider>();

            var contextBuilder = new ContextBuilder(startup, sessionStorageProviderMock.Object);

            contextBuilder.BuildServices();

            var actualValue = startup.Configuration.GetValue<string>(TestRuntimeStartup.CONFIGURATION_KEY);

            Assert.Equal(TestRuntimeStartup.CONFIGURATION_VALUE, actualValue);

            sessionStorageProviderMock.Verify(provider =>
                provider.StoreServiceProvider(It.Is<IServiceProvider>(provider =>
                    ServiceProviderIsConfigured(provider))));
        }

        [Fact]
        public void ShouldInitializeInstance()
        {
            var startup = new TestRuntimeStartup();
            var sessionStorageProviderMock = new Mock<ISessionStorageProvider>();
            var dependencyBinderMock = new Mock<IDependencyBinder>();
            var instance = new TestInstance(2);

            IServiceProvider serviceProvider =
                new ServiceCollection()
                    .AddTransient(_ => dependencyBinderMock.Object).BuildServiceProvider();

            sessionStorageProviderMock.Setup(provider =>
                provider.GetCurrentServiceProvider()).Returns(serviceProvider);

            var contextBuilder = new ContextBuilder(startup, sessionStorageProviderMock.Object);

            contextBuilder.InitializeCurrentInstance<TestRuntimeAttribute>(instance);

            dependencyBinderMock.Verify(binder =>
                binder.LoadFieldsByAttribute<TestRuntimeAttribute>(instance), Times.Once());

            dependencyBinderMock.Verify(binder =>
                binder.LoadPropertiesByAttribute<TestRuntimeAttribute>(instance), Times.Once());
        }

        private static bool ServiceProviderIsConfigured(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var dependencyBinder = serviceProvider.GetService<IDependencyBinder>();
            var testService = serviceProvider.GetService<ITestRuntimeService>();

            return configuration.GetValue<string>(TestRuntimeStartup.CONFIGURATION_KEY) == TestRuntimeStartup.CONFIGURATION_VALUE &&
                dependencyBinder is not null &&
                testService is not null;
        }
    }
}
