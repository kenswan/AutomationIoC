using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC.Tools
{
    public class AutomationSandboxTests
    {
        [Fact]
        public void ShouldRunCommand()
        {
            var context = AutomationSandbox.CreateContext<TestModule, TestStartup>();

            context.ConfigureServices(services =>
            {
                services.AddTransient<ITestService, TestService>();
            });

            var results = context.RunCommand();

            Assert.Single(results);
        }

        [Cmdlet(VerbsCommon.Get, "Test")]
        public class TestModule : IoCShell<TestStartup>
        {
            [AutomationDependency]
            protected readonly ITestService testService;

            protected override void ProcessRecord()
            {
                base.ProcessRecord();

                testService.CallTestMethod();
                testService.CallTestMethod();
                testService.CallTestMethod();

                WriteObject(testService.CallCount);
            }
        }

        public class TestStartup : IoCStartup
        {
            public override void Configure(IConfigurationBuilder configurationBuilder)
            {
                var appSettings = new Dictionary<string, string>()
                {
                    ["testOptions:mode"] = "basic-test",
                };

                configurationBuilder.AddInMemoryCollection(appSettings);
            }

            public override void ConfigureServices(IServiceCollection services)
            {
                services.AddTransient<ITestService, TestService>();
            }
        }

        public interface ITestService
        {
            int CallCount { get; }
            string CallTestMethod();
        }

        public class TestService : ITestService
        {
            public int CallCount { get; private set; }

            public string CallTestMethod()
            {
                CallCount += 1;

                return "This is a message Test";
            }
        }
    }
}
