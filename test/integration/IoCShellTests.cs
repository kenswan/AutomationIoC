using AutomationIoC.Commands;
using AutomationIoC.Tools;
using Xunit;

namespace AutomationIoC;

public class IoCShellTests
{
    [Fact]
    public void ShouldAddDependencies()
    {
        var expectedValue = 3;

        using var context = AutomationSandbox.CreateCommand<TestSDKCommand>();

        var actualValue = context.RunCommand<int>().FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }
}
