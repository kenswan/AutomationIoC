using AutomationIoC.Integration.Services;
using AutomationIoC.Runtime;
using AutomationIoC.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AutomationIoC
{
    public class TestAutomationContextTests
    {
        [Fact]
        public void ShouldLoadDependency()
        {
            var testServiceMock = new Mock<ITestService>();

            var testShell = TestAutomationContext.CreateInstance<TestIoCShell, TestStartup>(serviceCollection =>
            {
                serviceCollection.AddTransient(_ => testServiceMock.Object);
            });

            testShell.Execute();

            testServiceMock.Verify(service => service.CallTestMethod(), Times.Exactly(3));
        }

        public class TestIoCShell : IoCShell<TestStartup>
        {
            [AutomationDependency]
            protected readonly ITestService testService;

            protected override void BeginProcessing()
            {
                base.BeginProcessing();

                testService.CallTestMethod();
            }

            protected override void ProcessRecord()
            {
                base.ProcessRecord();

                testService.CallTestMethod();
            }

            protected override void EndProcessing()
            {
                base.EndProcessing();

                testService.CallTestMethod();
            }
        }

        public class TestStartup : IIoCStartup
        {
            public IConfiguration Configuration { get; set; }

            public void Configure(IConfigurationBuilder configurationBuilder)
            {
                throw new NotImplementedException();
            }

            public void ConfigureServices(IServiceCollection services)
            {
                throw new NotImplementedException();
            }
        }
    }
}
