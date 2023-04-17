// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.Consoles;

public interface ICommand
{
    void ConfigureCommand(Action<Command> configure);

    IDictionary<string, string> ConfigurationMapping { get; }

    Action<IServiceCollection> Services { get; }

    Action<IConfigurationBuilder> ConfigurationBuilder { get; }
}
