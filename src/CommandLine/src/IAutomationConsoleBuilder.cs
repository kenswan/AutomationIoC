// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.CommandLine;

/// <summary>
///     Automation command builder used to register commands
/// </summary>
public interface IAutomationConsoleBuilder
{
    /// <summary>
    ///     Add command to registered command path/pipeline
    /// </summary>
    /// <typeparam name="T">Type of command implementation</typeparam>
    /// <param name="commandPath">Path to register command (command and subcommand path)</param>
    /// <returns>Builder pattern object used for registration continuation</returns>
    IAutomationConsoleBuilder AddCommand<T>(params string[] commandPath) where T : IAutomationCommand, new();

    /// <summary>
    ///     Configure configuration for automation console
    /// </summary>
    /// <param name="configure">Configurations for services</param>
    IAutomationConsoleBuilder Configure(Action<HostBuilderContext, IConfigurationBuilder> configure);

    /// <summary>
    ///     Configure services for automation console
    /// </summary>
    /// <param name="configureServices">Service container configuration</param>
    IAutomationConsoleBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices);

    /// <summary>
    ///     CConfiguration mapping to use for automation console
    ///     This is used to map command line arguments to configuration keys
    /// </summary>
    /// <param name="configurationMapping">Argument -> Configuration Key Mapping</param>
    IAutomationConsoleBuilder WithConfigurationMapping(IDictionary<string, string> configurationMapping);

    /// <summary>
    ///     Finalize command registration and build automation console
    /// </summary>
    /// <returns>Automation console that will run registered process</returns>
    IAutomationConsole Build();
}
