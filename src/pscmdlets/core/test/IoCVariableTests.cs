// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoc.PSCmdlets.Integration.Commands;
using AutomationIoc.PSCmdlets.Tools;
using System.Management.Automation;
using Xunit;

namespace AutomationIoc.PSCmdlets;

public class IoCVariableTests
{
    [Fact]
    public void ShouldSetEnvironmentVariables()
    {
        var environmentKey = "TestKey";
        var expectedValue = Guid.NewGuid().ToString();

        using IAutomationCommand<TestVariableCommand> command = AutomationSandbox.CreateCommand<TestVariableCommand>();

        command.RunCommand(command =>
            command
                .AddParameter("Key", environmentKey)
                .AddParameter("Value", expectedValue));

        ICollection<PSVariable> results = command.RunExternalCommand<PSVariable>("Get-Variable", command =>
            command.AddParameter("Name", environmentKey));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First().Value);
    }
}
