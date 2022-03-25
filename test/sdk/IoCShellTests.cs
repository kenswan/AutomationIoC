using AutomationIoC.Models;
using AutomationIoC.Runtime;
using AutomationIoC.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AutomationIoC
{
    public class IoCShellTests
    {
        [Fact]
        public void ShouldLoadDependency()
        {
            var testServiceMock = new Mock<ITestSdkService>();

            var testShell = TestAutomationContext.CreateInstance<TestIoCShell, TestSDKStartup>(serviceCollection =>
            {
                serviceCollection.AddTransient(_ => testServiceMock.Object);
            });

            testShell.Execute();

            testServiceMock.Verify(service => service.RunMethod(), Times.Exactly(3));
        }

        internal class TestIoCShell : IoCShell<TestSDKStartup>
        {
            [AutomationDependency]
            protected ITestSdkService TestService { get; set; }

            protected override void BeginProcessing()
            {
                base.BeginProcessing();

                TestService.RunMethod();
            }

            protected override void ProcessRecord()
            {
                base.ProcessRecord();

                TestService.RunMethod();
            }

            protected override void EndProcessing()
            {
                base.EndProcessing();

                TestService.RunMethod();
            }
        }
    }
}
