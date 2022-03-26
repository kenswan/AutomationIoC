using AutomationIoC.Models;
using AutomationIoC.Tools;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC
{
    public class IoCShellTests
    {
        [Fact]
        public void ShouldLoadDependency()
        {
            var testServiceMock = new Mock<ITestSdkService>();

            var context = AutomationSandbox.CreateContext<TestIoCShell, TestSDKStartup>();

            context.ConfigureServices(serviceCollection =>
            {
                serviceCollection.AddTransient(_ => testServiceMock.Object);
            });

            context.RunCommand();

            testServiceMock.Verify(service => service.RunMethod(), Times.Exactly(3));
        }

        [Cmdlet(VerbsData.Mount, "Test")]
        public class TestIoCShell : IoCShell<TestSDKStartup>
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
