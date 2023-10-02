// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell.Tools;

/// <summary>
/// PowerShell automation command interface.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface IAutomationCommand<TCommand> : IDisposable where TCommand : PSCmdlet
{
    /// <summary>
    /// Add path for modules to import into sandboxed PowerShell session.
    /// </summary>
    /// <param name="modulePath">Path to module being imported</param>
    void ImportModule(string modulePath);

    /// <summary>
    /// Run PowerShell command
    /// </summary>
    /// <param name="buildCommand">Add additional properties and parameters to PowerShell command being run</param>
    /// <returns>Response object returned from isolated PowerShell process</returns>
    ICollection<PSObject> RunCommand(Action<PSCommand> buildCommand = null);

    /// <summary>
    /// Run command that returns strongly-typed response object
    /// </summary>
    /// <typeparam name="T">Type of return object coming from command completion</typeparam>
    /// <param name="buildCommand">Add additional properties and parameters to PowerShell command being run</param>
    /// <returns>Response object returned from isolated PowerShell process</returns>
    ICollection<T> RunCommand<T>(Action<PSCommand> buildCommand = null);

    /// <summary>
    /// Run external or built-in PowerShell Command
    /// </summary>
    /// <param name="name">Command name to run (i.e. Get-Help, Set-Location, Get-ChildItem, etc.)</param>
    /// <param name="buildCommand">Add additional properties and parameters to PowerShell command being run</param>
    /// <returns>Response object returned from isolated PowerShell process</returns>
    ICollection<PSObject> RunExternalCommand(string name, Action<PSCommand> buildCommand = null);

    /// <summary>
    /// Run external or built-in PowerShell Command
    /// </summary>
    /// <typeparam name="T">Type of return object coming from command completion</typeparam>
    /// <param name="name">Command name to run (i.e. Get-Help, Set-Location, Get-ChildItem, etc.)</param>
    /// <param name="buildCommand">Add additional properties and parameters to PowerShell command being run</param>
    /// <returns>Response object returned from isolated PowerShell process</returns>
    ICollection<T> RunExternalCommand<T>(string name, Action<PSCommand> buildCommand = null);

    /// <summary>
    /// Sets global variable to be used in PowerShell session.
    /// </summary>
    /// <param name="name">Name/Key of PowerShell variable</param>
    /// <param name="value">Value of PowerShell variable</param>
    void SetVariable(string name, object value);
}
