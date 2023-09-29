// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Management = System.Management.Automation;
using ManagementRunspace = System.Management.Automation.Runspaces;

namespace BlazorFocused.Automation.PowerShell.Tools.Command;

internal class NativeCommand : IDisposable
{
    protected readonly Management.PowerShell powerShellSession;
    protected readonly ManagementRunspace.Runspace runspace;
    private bool disposedResources;

    public NativeCommand(string modulePath)
    {
        var initial = ManagementRunspace.InitialSessionState.CreateDefault();

        if (modulePath is not null)
        {
            initial.ImportPSModule(new string[] { modulePath });
        }

        runspace = ManagementRunspace.RunspaceFactory.CreateRunspace(initial);
        runspace.Open();

        powerShellSession = Management.PowerShell.Create();
        powerShellSession.Runspace = runspace;
    }

    public void ImportModule(string modulePath) => InvokeCommand<Management.PSObject>("Import-Module", command => command.AddParameter("Name", modulePath));

    public ICollection<T> InvokeCommand<T>(string commandName, Action<Management.PSCommand> buildCommand)
    {
        powerShellSession.Commands.Clear();

        Management.PSCommand command = powerShellSession.Commands.AddCommand(commandName);

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
