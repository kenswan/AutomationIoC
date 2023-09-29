// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using BlazorFocused.Automation.PowerShell.Tools.Command;
using BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Commands;

namespace BlazorFocused.Automation.PowerShell.Tools.Test.Command;

public class AutomationCommandTests
{
    [Fact]
    public void ShouldRunExternalCommands()
    {
        string environmentKey = Guid.NewGuid().ToString();
        string expectedValue = Guid.NewGuid().ToString();

        using var automationCommand = new AutomationCommand<TestCommand>();

        automationCommand.RunExternalCommand("Set-Variable", command =>
            command
                .AddParameter("Name", environmentKey)
                .AddParameter("Value", expectedValue));

        ICollection<PSVariable> results = automationCommand.RunExternalCommand<PSVariable>("Get-Variable", command =>
            command.AddParameter("Name", environmentKey));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First().Value);
    }
}
