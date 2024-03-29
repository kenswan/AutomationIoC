﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Integration.Commands;
using AutomationIoC.PSCmdlets.Tools;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC.PSCmdlets;

public class IoCVariableTests
{
    [Fact]
    public void ShouldSetEnvironmentVariables()
    {
        string environmentKey = "TestKey";
        string expectedValue = Guid.NewGuid().ToString();

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
