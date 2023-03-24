// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Xunit;

namespace AutomationIoC.PSCmdlets.Tools;

public class EnvironmentTests
{
    protected readonly PowerShell powerShellSession;
    protected readonly Runspace runspace;

    [Fact]
    public void Test()
    {
        var key = "TestKey";
        var value = "TestValue";

        var initial = InitialSessionState.CreateDefault();
        Runspace runspace = RunspaceFactory.CreateRunspace(initial);
        runspace.Open();

        var powerShellSession = PowerShell.Create();
        powerShellSession.Runspace = runspace;

        powerShellSession.Commands.AddCommand("Set-Variable")
                .AddParameter("Name", key)
                .AddParameter("Value", value);

        powerShellSession.Invoke();

        powerShellSession.Commands.Clear();

        powerShellSession.Commands.AddCommand("Get-Variable").AddParameter("Name", key);

        System.Collections.ObjectModel.Collection<PSObject> results = powerShellSession.Invoke();

        Assert.Single(results);
    }
}
