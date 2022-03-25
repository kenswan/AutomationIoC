using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Models;
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
            var dependencyContext = new DependencyContext<TestRuntimeAttribute, TestRuntimeStartup>()
            {
                Instance = new TestInstance(2)
            };
            var contextBuilderMock = new Mock<IContextBuilder>();
            contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(false);

            IServiceProvider serviceProvider =
                new ServiceCollection()
                    .AddTransient(_ => contextBuilderMock.Object)
                    .BuildServiceProvider();

            new AutomationIoCBinder(serviceProvider).BindContext(dependencyContext);

            contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Once);

            contextBuilderMock.Verify(builder =>
                builder.InitializeCurrentInstance<TestRuntimeAttribute>(dependencyContext.Instance),
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

            var dependencyContext = new DependencyContext<TestRuntimeAttribute, TestRuntimeStartup>()
            {
                Instance = new TestInstance(2)
            };

            new AutomationIoCBinder(serviceProvider).BindContext(dependencyContext);

            contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Never);

            contextBuilderMock.Verify(builder =>
                builder.InitializeCurrentInstance<TestRuntimeAttribute>(dependencyContext.Instance),
                    Times.Once);
        }

        [Fact]
        public void ShouldBindExistingServiceCollection()
        {
            var dependencyBinderMock = new Mock<IDependencyBinder>();
            var instance = new TestInstance(2);

            IServiceCollection serviceCollection =
                new ServiceCollection()
                    .AddTransient(_ => dependencyBinderMock.Object);

            new AutomationIoCBinder(serviceCollection.BuildServiceProvider())
                .BindServiceCollection<TestRuntimeAttribute>(serviceCollection, instance);

            dependencyBinderMock.Verify(binder =>
                binder.LoadFieldsByAttribute<TestRuntimeAttribute>(instance),
                    Times.Once);

            dependencyBinderMock.Verify(binder =>
                binder.LoadPropertiesByAttribute<TestRuntimeAttribute>(instance),
                    Times.Once);
        }
    }
}
