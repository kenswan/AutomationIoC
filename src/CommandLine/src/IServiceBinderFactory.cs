﻿// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.CommandLine;

/// <summary>
///     Factory of services used to bind to command line services
/// </summary>
public interface IServiceBinderFactory
{
    /// <summary>
    ///     Binds registered services to registered command handlers
    /// </summary>
    /// <typeparam name="T">Type of Service needed for implementation</typeparam>
    /// <returns>Custom type binding for downstream dependency injection</returns>
    public T Bind<T>();
}
