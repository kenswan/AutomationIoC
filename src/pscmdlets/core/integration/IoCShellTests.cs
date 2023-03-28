// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Integration.Commands;
using AutomationIoC.PSCmdlets.Tools;
using Xunit;

namespace AutomationIoC.PSCmdlets.Integration;

public class IoCShellTests
{
    [Fact]
    public void ShouldAddDependencies()
    {
        var expectedValue = 3;

        using IAutomationCommand<TestPSCmdletCommand> context = AutomationSandbox.CreateCommand<TestPSCmdletCommand>();

        var actualValue = context.RunCommand<int>().FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }
}
