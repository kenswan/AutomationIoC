using AutomationIoC.Context;
using AutomationIoC.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace AutomationIoC
{
    public class IoCShellBaseTests
    {
        [Fact]
        public void ShouldInjectDependencies()
        {
            var automationContext = new Mock<IAutomationContext>();
            var expectedTestService = new TestService();

            automationContext.Setup(context =>
                context.GetDependency(typeof(TestService)))
                    .Returns(expectedTestService);

            var testDependencyInjection = new TestDependencyInjection();

            testDependencyInjection.Context = automationContext.Object;

            Assert.Null(testDependencyInjection.TestService);

            testDependencyInjection.LoadDependencies();

            var actualTestService = testDependencyInjection.TestService;

            actualTestService.Should().NotBeNull()
                .And.BeEquivalentTo(expectedTestService);
        }

        private class TestDependencyInjection : IoCShellBase
        {
            [AutoInject]
            public TestService TestService { get; set; }
        }
    }
}
