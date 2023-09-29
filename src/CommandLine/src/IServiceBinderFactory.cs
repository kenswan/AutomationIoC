// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine.Binding;

namespace BlazorFocused.Automation.CommandLine;

/// <summary>
///
/// </summary>
public interface IServiceBinderFactory
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public BinderBase<T> Bind<T>();
}
