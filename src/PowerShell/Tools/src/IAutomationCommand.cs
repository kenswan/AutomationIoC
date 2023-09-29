// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell.Tools;

/// <summary>
///
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface IAutomationCommand<TCommand> : IDisposable where TCommand : PSCmdlet
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="modulePath"></param>
    void ImportModule(string modulePath);

    /// <summary>
    ///
    /// </summary>
    /// <param name="buildCommand"></param>
    /// <returns></returns>
    ICollection<PSObject> RunCommand(Action<PSCommand> buildCommand = null);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="buildCommand"></param>
    /// <returns></returns>
    ICollection<T> RunCommand<T>(Action<PSCommand> buildCommand = null);

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="buildCommand"></param>
    /// <returns></returns>
    ICollection<PSObject> RunExternalCommand(string name, Action<PSCommand> buildCommand = null);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="buildCommand"></param>
    /// <returns></returns>
    ICollection<T> RunExternalCommand<T>(string name, Action<PSCommand> buildCommand = null);

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void SetVariable(string name, object value);
}
