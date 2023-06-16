// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.Consoles;

public interface IConsoleCommand
{
    void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command);

    IDictionary<string, string> ConfigurationMapping { get; }

    Action<IServiceCollection> Services { get; }

    Action<IConfigurationBuilder> ConfigurationBuilder { get; }
}
