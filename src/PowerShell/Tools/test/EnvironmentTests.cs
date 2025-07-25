// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation.Runspaces;
using Management = System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools.Test;

public class EnvironmentTests
{
    protected readonly Management.PowerShell powerShellSession;
    protected readonly Runspace runspace;

    [Fact]
    public void Test()
    {
        string key = "TestKey";
        string value = "TestValue";

        var initial = InitialSessionState.CreateDefault();
        Runspace runspace = RunspaceFactory.CreateRunspace(initial);
        runspace.Open();

        var powerShellSession = Management.PowerShell.Create();
        powerShellSession.Runspace = runspace;

        powerShellSession.Commands.AddCommand("Set-Variable")
                .AddParameter("Name", key)
                .AddParameter("Value", value);

        powerShellSession.Invoke();

        powerShellSession.Commands.Clear();

        powerShellSession.Commands.AddCommand("Get-Variable").AddParameter("Name", key);

        System.Collections.ObjectModel.Collection<Management.PSObject> results = powerShellSession.Invoke();

        Assert.Single(results);
    }
}
