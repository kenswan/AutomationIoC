using Moq;
using AutomationIoC.Services;
using System.Management.Automation;
using Xunit;
using AutomationIoC.Context;

namespace AutomationIoC
{
    public class IoCShellTests
    {
        [Fact]
        public void ShouldCallExecuteMethod()
        {
            var automationContext = new Mock<IAutomationContext>();
            var testService = new TestService();

            automationContext.Setup(context =>
                context.GetDependency(typeof(TestService)))
                    .Returns(testService);

            var commandRuntimeMock = new Mock<ICommandRuntime>();

            var psCmdlet = new TestShell()
            {
                Context = automationContext.Object,
                CommandRuntime = commandRuntimeMock.Object
            };

            psCmdlet.RunInstance();
            psCmdlet.RunInstance();
            psCmdlet.RunInstance();

            Assert.Equal(3, testService.CallCount);
        }

        [Cmdlet(VerbsCommon.Get, "Test")]
        public class TestShell : IoCShell
        {
            [AutoInject]
            public TestService TestService { get; set; }

            protected override void ExecuteCmdlet()
            {
                TestService.CallTestMethod();
            }
        }
    }
}
