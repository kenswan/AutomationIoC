// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Test.TestBed.Commands;
using BlazorFocused.Automation.PowerShell.Test.TestBed.Startup;
using BlazorFocused.Automation.PowerShell.Tools;

namespace BlazorFocused.Automation.PowerShell.Test;

public partial class AutomationShellTests
{
    [Fact]
    public void ShouldAddDependencies()
    {
        int expectedValue = 3;

        using IPowerShellAutomation<TestStartup> context = AutomationSandbox.CreateSession<TestStartup>();

        int actualValue = context.RunAutomationCommand<TestPSCmdletCommand, int>().FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }
}
