using AutomationIoC.Context;
using AutomationIoC.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC
{
    public class AutomationStartupTests
    {
        [Fact]
        public void ShouldExecuteStartupMethods()
        {
            var automationContext = new Mock<IAutomationContext>();
            var commandRuntimeMock = new Mock<ICommandRuntime>();

            var automationStartup = new TestStartup
            {
                Context = automationContext.Object,
                CommandRuntime = commandRuntimeMock.Object
            };

            automationStartup.RunInstance();

            automationContext.Verify(context =>
                context.GenerateServices(It.Is<IServiceCollection>(collection =>
                    ContainsServices(collection))), Times.Once);
        }

        private bool ContainsServices(IServiceCollection serviceCollection)
        {
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var configuration = serviceProvider.GetService<IConfiguration>();
            var testService = serviceProvider.GetService<TestService>();

            return configuration is not null && testService is not null;
        }

        [Cmdlet(VerbsLifecycle.Build, "Dependencies")]
        public class TestStartup : AutomationStartup
        {
            private readonly TestService testService;

            public TestStartup()
            {
                testService = new();
            }

            public int CallCount => testService.CallCount;

            public override void Configure(IConfigurationBuilder configurationBuilder)
            {
                _ = testService.CallTestMethod();
            }

            public override void ConfigureServices(IServiceCollection services)
            {
                services.AddTransient(_ => testService);

                _ = testService.CallTestMethod();
            }
        }
    }
}