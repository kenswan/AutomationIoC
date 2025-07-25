// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Test.TestBed.Commands;
using AutomationIoC.PowerShell.Test.TestBed.Startup;
using AutomationIoC.PowerShell.Tools;

namespace AutomationIoC.PowerShell.Test;

public partial class AutomationShellTests
{
    [Fact]
    public void ShouldAddDependencies()
    {
        int expectedValue = 3;

        using IPowerShellAutomation<TestStartup> context = AutomationSandbox.CreateSession<TestStartup>();

        int actualValue = context.RunAutomationCommand<TestAutomationShellCommand, int>().FirstOrDefault();

        Assert.Equal(expectedValue, actualValue);
    }
}
