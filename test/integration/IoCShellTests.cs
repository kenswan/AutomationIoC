using AutomationIoC.Integration.Commands;
using AutomationIoC.Tools;
using Xunit;

namespace AutomationIoC.Integration;

public class IoCShellTests
{
    [Fact]
    public void ShouldAddDependencies()
    {
        var expectedValue = 3;

        using IAutomationCommand<TestSDKCommand> context = AutomationSandbox.CreateCommand<TestSDKCommand>();

        var actualValue = context.RunCommand<int>().FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }
}
