// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Tools.Context;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Commands;
using Bogus;
using System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools.Test.Context;

public partial class PowerShellAutomationContextTests
{
    [Fact]
    public void RunCommand_ShouldRunBuiltInCommandWithGenericOutput()
    {
        string environmentKey = Guid.NewGuid().ToString();
        string expectedValue = Guid.NewGuid().ToString();

        using var powerShellAutomationContext = new PowerShellAutomationContext();

        powerShellAutomationContext.RunCommand("Set-Variable", command =>
            command
                .AddParameter("Name", environmentKey)
                .AddParameter("Value", expectedValue));

        ICollection<PSObject> results = powerShellAutomationContext.RunCommand("Get-Variable", command =>
            command.AddParameter("Name", environmentKey));

        Assert.Single(results);
    }

    [Fact]
    public void RunCommand_ShouldRunBuiltInCommandWithSpecificOutput()
    {
        string environmentKey = Guid.NewGuid().ToString();
        string expectedValue = Guid.NewGuid().ToString();

        using var powerShellAutomationContext = new PowerShellAutomationContext();

        powerShellAutomationContext.RunCommand("Set-Variable", command =>
            command
                .AddParameter("Name", environmentKey)
                .AddParameter("Value", expectedValue));

        ICollection<PSVariable> results = powerShellAutomationContext.RunCommand<PSVariable>("Get-Variable", command =>
            command.AddParameter("Name", environmentKey));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First().Value);
    }

    [Fact]
    public void RunCommand_ShouldRunImportedPSCmdlet()
    {
        string inputParameter = new Faker().Random.AlphaNumeric(10);
        string expectedValue = TestPSCmdletCommand.TransformOutput(inputParameter);

        using var powerShellAutomationContext = new PowerShellAutomationContext();

        powerShellAutomationContext.ImportModule(typeof(TestPSCmdletCommand).Assembly.Location);

        ICollection<PSObject> results = powerShellAutomationContext.RunCommand<TestPSCmdletCommand>(command =>
            command.AddParameter("Test", inputParameter));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First().BaseObject.ToString());
    }

    [Fact]
    public void RunCommand_ShouldImportPSCmdletIfDoesNotExist()
    {
        string inputParameter = new Faker().Random.AlphaNumeric(10);
        string expectedValue = TestPSCmdletCommand.TransformOutput(inputParameter);

        using var powerShellAutomationContext = new PowerShellAutomationContext();

        ICollection<PSObject> results = powerShellAutomationContext.RunCommand<TestPSCmdletCommand>(command =>
            command.AddParameter("Test", inputParameter));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First().BaseObject.ToString());
    }
}
