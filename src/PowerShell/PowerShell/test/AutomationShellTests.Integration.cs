// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Test.TestBed.Commands;
using BlazorFocused.Automation.PowerShell.Tools;

namespace BlazorFocused.Automation.PowerShell.Test;

public partial class AutomationShellTests
{
    [Fact]
    public void ShouldAddDependencies()
    {
        int expectedValue = 3;

        using IAutomationCommand<TestPSCmdletCommand> context =
            AutomationSandbox.CreateCommand<TestPSCmdletCommand>();

        int actualValue = context.RunCommand<int>().FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }
}
