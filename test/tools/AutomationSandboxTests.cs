using AutomationIoC.Integration.Commands;
using AutomationIoC.Integration.Services;
using AutomationIoC.Integration.Startup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AutomationIoC.Tools;

public class AutomationSandboxTests
{
    [Fact]
    public void ShouldRunCommandContext()
    {
        using IAutomationCommand<TestContextCommand> context = AutomationSandbox.CreateContext<TestContextCommand, TestStartup>(services =>
        {
            services.AddTransient<ITestService, TestService>();
        });

        ICollection<System.Management.Automation.PSObject> results = context.RunCommand();

        Assert.Single(results);
    }

    [Fact]
    public void ShouldRunCommand()
    {
        var expectedValue = Guid.NewGuid().ToString();

        using IAutomationCommand<TestCommand> command = AutomationSandbox.CreateCommand<TestCommand>();

        ICollection<System.Management.Automation.PSObject> results = command.RunCommand(command => command.AddParameter("Test", expectedValue));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First());
    }
}
