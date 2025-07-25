// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools;

/// <summary>
/// PowerShell automation interface.
/// </summary>
public interface IPowerShellAutomation : IDisposable
{
    /// <summary>
    /// Add path for modules to import into sandboxed PowerShell session.
    /// </summary>
    /// <param name="modulePath">Path to module being imported</param>
    void ImportModule(string modulePath);

    /// <summary>
    /// Run external or built-in PowerShell Command
    /// </summary>
    /// <param name="name">Command name to run (i.e. Get-Help, Set-Location, Get-ChildItem, etc.)</param>
    /// <param name="buildCommand">Add additional properties and parameters to PowerShell command being run</param>
    /// <returns>Response object returned from isolated PowerShell process</returns>
    ICollection<PSObject> RunCommand(string name, Action<PSCommand> buildCommand = null);

    /// <summary>
    /// Run external or built-in PowerShell Command
    /// </summary>
    /// <typeparam name="T">Type of return object coming from command completion</typeparam>
    /// <param name="name">Command name to run (i.e. Get-Help, Set-Location, Get-ChildItem, etc.)</param>
    /// <param name="buildCommand">Add additional properties and parameters to PowerShell command being run</param>
    /// <returns>Deserialized response object returned from isolated PowerShell process</returns>
    ICollection<T> RunCommand<T>(string name, Action<PSCommand> buildCommand = null);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TPSCmdlet"></typeparam>
    /// <param name="buildCommand"></param>
    /// <returns></returns>
    ICollection<PSObject> RunCommand<TPSCmdlet>(Action<PSCommand> buildCommand = null)
        where TPSCmdlet : PSCmdlet;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TPSCmdlet"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="buildCommand"></param>
    /// <returns></returns>
    ICollection<TOutput> RunCommand<TPSCmdlet, TOutput>(Action<PSCommand> buildCommand = null)
        where TPSCmdlet : PSCmdlet;

    /// <summary>
    /// Sets global variable to be used in PowerShell session.
    /// </summary>
    /// <param name="name">Name/Key of PowerShell variable</param>
    /// <param name="value">Value of PowerShell variable</param>
    void SetVariable(string name, object value);
}
