// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using System.Reflection;
using ManagementRunspace = System.Management.Automation.Runspaces;

namespace AutomationIoC.PowerShell.Tools.Context;

internal class PowerShellAutomationContext : IPowerShellAutomation, IDisposable
{
    protected readonly System.Management.Automation.PowerShell powerShellSession;
    protected readonly ManagementRunspace.Runspace runspace;
    private bool disposedResources;

    public PowerShellAutomationContext()
    {
        var initial = ManagementRunspace.InitialSessionState.CreateDefault();

        runspace = ManagementRunspace.RunspaceFactory.CreateRunspace(initial);
        runspace.Open();

        powerShellSession = System.Management.Automation.PowerShell.Create();
        powerShellSession.Runspace = runspace;
    }

    public void ImportModule(string modulePath)
    {
        ICollection<PSObject> test = RunCommand<PSObject>("Import-Module", command =>
            command.AddParameter("Name", modulePath));

        Console.WriteLine(test.Count);
    }

    public ICollection<PSObject> RunCommand(string commandName, Action<PSCommand> buildCommand) =>
        RunCommand<PSObject>(commandName, buildCommand);

    public ICollection<T> RunCommand<T>(string commandName, Action<PSCommand> buildCommand)
    {
        powerShellSession.Commands.Clear();

        PSCommand command = powerShellSession.Commands.AddCommand(commandName);

        if (buildCommand is not null)
        {
            buildCommand(command);
        }

        return powerShellSession.Invoke<T>();
    }

    public ICollection<PSObject> RunCommand<TPSCmdlet>(Action<PSCommand> buildCommand = null)
        where TPSCmdlet : PSCmdlet =>
            RunCommand<TPSCmdlet, PSObject>(buildCommand);

    public ICollection<TOutput> RunCommand<TPSCmdlet, TOutput>(Action<PSCommand> buildCommand = null)
        where TPSCmdlet : PSCmdlet
    {
        string commandName = GetCommandName<TPSCmdlet>();

        ImportPSCmdletModule<TPSCmdlet>();

        return RunCommand<TOutput>(commandName, buildCommand);
    }

    public void SetVariable(string name, object value) =>
        RunCommand("Set-Variable", command => command.AddParameter("Name", name).AddParameter("Value", value));

    protected static string GetCommandName<TCommand>() =>
            Attribute.GetCustomAttribute(typeof(TCommand), typeof(CmdletAttribute)) is not CmdletAttribute cmdletAttribute
                ? throw new ArgumentException("CmdletAttribute not found on class", nameof(cmdletAttribute))
                : $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";

    protected void ImportPSCmdletModule<TImportClass>()
    {
        Assembly importAssembly = typeof(TImportClass).Assembly;
        string importAssemblyName = importAssembly.GetName().Name;
        string importAssemblyLocation = importAssembly.Location;

        ICollection<PSModuleInfo> modules = RunCommand<PSModuleInfo>("Get-Module", command =>
                   command.AddParameter("Name", importAssemblyName));

        if (modules.Count == 0)
        {
            ImportModule(importAssemblyLocation);
        }
    }

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
