// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Integration.Commands;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC.PSCmdlets.Tools.Command;

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
