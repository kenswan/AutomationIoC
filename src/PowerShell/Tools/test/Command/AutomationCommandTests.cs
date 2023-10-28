// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Tools.Context;
using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell.Tools.Test.Command;

public class AutomationCommandTests
{
    [Fact]
    public void ShouldRunExternalCommands()
    {
        string environmentKey = Guid.NewGuid().ToString();
        string expectedValue = Guid.NewGuid().ToString();

        using var automationCommand = new PowerShellAutomationContext();

        automationCommand.RunCommand("Set-Variable", command =>
            command
                .AddParameter("Name", environmentKey)
                .AddParameter("Value", expectedValue));

        ICollection<PSVariable> results = automationCommand.RunCommand<PSVariable>("Get-Variable", command =>
            command.AddParameter("Name", environmentKey));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First().Value);
    }
}
