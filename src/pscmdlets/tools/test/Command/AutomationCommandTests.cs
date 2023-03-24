// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoc.PSCmdlets.Integration.Commands;
using System.Management.Automation;
using Xunit;

namespace AutomationIoc.PSCmdlets.Tools.Command;

public class AutomationCommandTests
{
    [Fact]
    public void ShouldRunExternalCommands()
    {
        var environmentKey = Guid.NewGuid().ToString();
        var expectedValue = Guid.NewGuid().ToString();

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
