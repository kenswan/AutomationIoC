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
        using var context = AutomationSandbox.CreateContext<TestContextCommand, TestStartup>(services =>
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

        using var command = AutomationSandbox.CreateCommand<TestCommand>();

        var results = command.RunCommand(command => command.AddParameter("Test", expectedValue));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First());
    }
}
