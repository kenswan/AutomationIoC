using AutomationIoC.Integration.Commands;
using AutomationIoC.Tools;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC;

public class IoCVariableTests
{
    [Fact]
    public void ShouldSetEnvironmentVariables()
    {
        var environmentKey = "TestKey";
        var expectedValue = Guid.NewGuid().ToString();

        using var command = AutomationSandbox.CreateCommand<TestVariableCommand>();

        command.RunCommand(command =>
            command
                .AddParameter("Key", environmentKey)
                .AddParameter("Value", expectedValue));

        var results = command.RunExternalCommand<PSVariable>("Get-Variable", command =>
            command.AddParameter("Name", environmentKey));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First().Value);
    }
}
