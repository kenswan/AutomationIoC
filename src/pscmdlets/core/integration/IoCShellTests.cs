// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoc.PSCmdlets.Integration.Commands;
using AutomationIoc.PSCmdlets.Tools;
using Xunit;

namespace AutomationIoc.PSCmdlets.Integration;

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
