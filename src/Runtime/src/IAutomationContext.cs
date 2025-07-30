// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;

namespace AutomationIoC.Runtime;

/// <summary>
///     Service Container and Configurations for current automation process.
/// </summary>
public interface IAutomationContext
{
    /// <summary>
    ///     Returns the configuration used within the current process.
    /// </summary>
    /// <returns>Current Scoped Configuration</returns>
    IConfiguration Configuration { get; }

    /// <summary>
    ///     Returns the service provider used to resolve services within the current process.
    /// </summary>
    /// <returns>Current Scoped Service Provider</returns>
    IServiceProvider ServiceProvider { get; }
}
