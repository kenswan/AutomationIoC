using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC.Tools
{
    public class AutomationSandboxTests
    {
        [Fact]
        public void ShouldRunCommandContext()
        {
            using var context = AutomationSandbox.CreateContext<TestModuleContext, TestStartup>();

            context.ConfigureServices(services =>
            {
                services.AddTransient<ITestService, TestService>();
            });

            var results = context.RunCommand();

            Assert.Single(results);
        }

        [Fact]
        public void ShouldRunCommand()
        {
            var expectedValue = Guid.NewGuid().ToString();

            using var context = AutomationSandbox.CreateCommand<TestModuleCommand>();

            context.ConfigureParameters(command => command.AddParameter("Test", expectedValue));
            var results = context.RunCommand();

            Assert.Single(results);
            Assert.Equal(expectedValue, results.First());
        }

        [Cmdlet(VerbsCommon.Get, "Test")]
        public class TestModuleContext : PSCmdlet
        {
            [Tools]
            protected readonly ITestService testService;

            protected override void BeginProcessing()
            {
                base.BeginProcessing();

                var dependencyContext = new DependencyContext<ToolsAttribute, TestStartup>
                {
                    Instance = this,
                    SessionState = SessionState
                };

                AutomationIoCRuntime.BindContext(dependencyContext);

            }

            protected override void ProcessRecord()
            {
                base.ProcessRecord();

                testService.CallTestMethod();
                testService.CallTestMethod();
                testService.CallTestMethod();

                WriteObject(testService.CallCount);
            }
        }

        [Cmdlet(VerbsCommon.Get, "TestCommand")]
        public class TestModuleCommand : PSCmdlet
        {
            [Parameter(Mandatory = true)]
            public string Test;

            protected override void ProcessRecord()
            {
                base.ProcessRecord();

                WriteObject(Test);
            }
        }

        public class TestStartup : IIoCStartup
        {
            public IConfiguration Configuration { get; set; }

            public IAutomationEnvironment AutomationEnvironment { get; set; }

            public void Configure(IConfigurationBuilder configurationBuilder)
            {
                var appSettings = new Dictionary<string, string>()
                {
                    ["testOptions:mode"] = "basic-test",
                };

                configurationBuilder.AddInMemoryCollection(appSettings);
            }

            public void ConfigureServices(IServiceCollection services)
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

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public class ToolsAttribute : Attribute { }
    }
}
