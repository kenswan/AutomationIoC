using AutomationIoC.Context;
using AutomationIoC.Services;
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
            var testServiceMock = new Mock<ITestService>();
            var testServiceForPropertyMock = new Mock<ITestServiceForProperty>();

            automationContext.Setup(context =>
                context.GetDependency(typeof(ITestService)))
                    .Returns(testServiceMock.Object);

            automationContext.Setup(context =>
                context.GetDependency(typeof(ITestServiceForProperty)))
                    .Returns(testServiceForPropertyMock.Object);

            var testIoCShellBase = new TestIoCShellBase
            {
                Context = automationContext.Object
            };

            testIoCShellBase.RunInstance();

            testServiceMock.Verify(service =>
                service.CallTestMethod(), Times.Once);

            testServiceForPropertyMock.Verify(service =>
                service.CallTestMethod(), Times.Once);
        }

        private class TestIoCShellBase : IoCShellBase
        {
            [AutomationDependency]
            protected ITestServiceForProperty TestServiceForProperty { get; set; }

            [AutomationDependency]
            private readonly ITestService testService = default;

            protected override void ProcessRecord()
            {
                testService.CallTestMethod();

                TestServiceForProperty.CallTestMethod();
            }
        }
    }
}
