// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.Consoles;

public abstract class BaseCommand : ICommand
{
    public string ApplicationName => RootCommand.ExecutableName;

    public abstract void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command);

    public virtual IDictionary<string, string> ConfigurationMapping { get; }

    public virtual Action<IServiceCollection> Services { get; }

    public virtual Action<IConfigurationBuilder> ConfigurationBuilder { get; }
}
