// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime;

/// <summary>
/// Startup class for automation runtime
/// </summary>
public interface IAutomationStartup
{
    /// <summary>
    /// Sets Configuration for automation runtime
    /// </summary>
    /// <param name="hostBuilderContext"></param>
    /// <param name="configurationBuilder"></param>
    void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder);

    /// <summary>
    /// Establishes services for automation runtime
    /// </summary>
    /// <param name="hostBuilderContext"></param>
    /// <param name="services"></param>
    void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services);

    /// <summary>
    /// Produces a list of parameters that can be used to configure the automation runtime
    /// </summary>
    /// <returns>List of parameters that can be used to configure the automation runtime</returns>
    /// <remarks>
    /// <code>[ "--environment" , "Staging" ]</code> The above would set the <see cref="IHostEnvironment.EnvironmentName"/>
    /// to use during <see cref="Configure"/> method to alter/add configuration files (i.e. appsettings.{Environment}.json)
    /// </remarks>
    string[] GenerateParameters();

    /// <summary>
    /// Mappings used to bind parameter values with <see cref="IConfiguration" /> keys
    /// </summary>
    /// <returns>Parameter to <see cref="IConfiguration" /> bindings</returns>
    IDictionary<string, string> GenerateParameterConfigurationMapping();
}
