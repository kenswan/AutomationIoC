// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AutomationIoC.PSCmdlets.Tools.Command;

internal class NativeCommand : IDisposable
{
    protected readonly PowerShell powerShellSession;
    protected readonly Runspace runspace;
    private bool disposedResources;

    public NativeCommand(string modulePath)
    {
        var initial = InitialSessionState.CreateDefault();

        if (modulePath is not null)
        {
            initial.ImportPSModule(new string[] { modulePath });
        }

        runspace = RunspaceFactory.CreateRunspace(initial);
        runspace.Open();

        powerShellSession = PowerShell.Create();
        powerShellSession.Runspace = runspace;
    }

    public void ImportModule(string modulePath) => InvokeCommand<PSObject>("Import-Module", command => command.AddParameter("Name", modulePath));

    public ICollection<T> InvokeCommand<T>(string commandName, Action<PSCommand> buildCommand)
    {
        powerShellSession.Commands.Clear();

        PSCommand command = powerShellSession.Commands.AddCommand(commandName);

        if (buildCommand is not null)
        {
            buildCommand(command);
        }

        return powerShellSession.Invoke<T>();
    }

    ~NativeCommand()
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
