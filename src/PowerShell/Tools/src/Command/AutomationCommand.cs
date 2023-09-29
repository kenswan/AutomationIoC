// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell.Tools.Command;

internal class AutomationCommand<TCommand> : NativeCommand, IAutomationCommand<TCommand>
    where TCommand : PSCmdlet
{
    protected readonly string commandName;

    public AutomationCommand() : base(typeof(TCommand).Assembly.Location)
    {
        commandName = GetCommandName();
    }

    public void SetVariable(string name, object value) =>
        RunExternalCommand("Set-Variable", command => command.AddParameter("Name", name).AddParameter("Value", value));

    public ICollection<PSObject> RunCommand(Action<PSCommand> buildCommand = null) =>
        InvokeCommand<PSObject>(commandName, buildCommand);

    public ICollection<T> RunCommand<T>(Action<PSCommand> buildCommand = null) => InvokeCommand<T>(commandName, buildCommand);

    public ICollection<PSObject> RunExternalCommand(string name, Action<PSCommand> buildCommand = null) =>
        InvokeCommand<PSObject>(name, buildCommand);

    public ICollection<T> RunExternalCommand<T>(string name, Action<PSCommand> buildCommand = null) => InvokeCommand<T>(name, buildCommand);

    private static string GetCommandName() =>
        Attribute.GetCustomAttribute(typeof(TCommand), typeof(CmdletAttribute)) is not CmdletAttribute cmdletAttribute
            ? throw new ArgumentException("CmdletAttribute not found on class", nameof(cmdletAttribute))
            : $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";
}
