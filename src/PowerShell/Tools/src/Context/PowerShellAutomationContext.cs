// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using Management = System.Management.Automation;
using ManagementRunspace = System.Management.Automation.Runspaces;

namespace BlazorFocused.Automation.PowerShell.Tools.Context;

internal class PowerShellAutomationContext : IPowerShellAutomation, IDisposable
{
    protected readonly Management.PowerShell powerShellSession;
    protected readonly ManagementRunspace.Runspace runspace;
    private bool disposedResources;

    public PowerShellAutomationContext()
    {
        var initial = ManagementRunspace.InitialSessionState.CreateDefault();

        runspace = ManagementRunspace.RunspaceFactory.CreateRunspace(initial);
        runspace.Open();

        powerShellSession = Management.PowerShell.Create();
        powerShellSession.Runspace = runspace;
    }

    public void ImportModule(string modulePath) =>
        RunCommand<Management.PSObject>("Import-Module", command =>
            command.AddParameter("Name", modulePath));

    public ICollection<Management.PSObject> RunCommand(string commandName, Action<Management.PSCommand> buildCommand) =>
        RunCommand<Management.PSObject>(commandName, buildCommand);

    public ICollection<T> RunCommand<T>(string commandName, Action<Management.PSCommand> buildCommand)
    {
        powerShellSession.Commands.Clear();

        Management.PSCommand command = powerShellSession.Commands.AddCommand(commandName);

        if (buildCommand is not null)
        {
            buildCommand(command);
        }

        return powerShellSession.Invoke<T>();
    }

    public ICollection<PSObject> RunCommand<TPSCmdlet>(Action<PSCommand> buildCommand = null) where TPSCmdlet : PSCmdlet => throw new NotImplementedException();

    public ICollection<TOutput> RunCommand<TPSCmdlet, TOutput>(Action<PSCommand> buildCommand = null) where TPSCmdlet : PSCmdlet => throw new NotImplementedException();

    public void SetVariable(string name, object value) =>
        RunCommand("Set-Variable", command => command.AddParameter("Name", name).AddParameter("Value", value));

    ~PowerShellAutomationContext()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);

        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing || !disposedResources)
        {
            if (runspace is not null)
            {
                runspace.Close();
                runspace.Dispose();
            }

            if (powerShellSession is not null)
            {
                powerShellSession.Stop();
                powerShellSession.Dispose();
            }
        }

        disposedResources = true;
    }
}
