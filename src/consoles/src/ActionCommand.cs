// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.Consoles;

public abstract class ActionCommand : ICommand
{
    public abstract void ConfigureCommand(Action<Command> configure);

    public virtual IDictionary<string, string> ConfigurationMapping { get; private set; }

    public virtual Action<IServiceCollection> Services { get; private set; }

    public virtual Action<IConfigurationBuilder> ConfigurationBuilder { get; private set; }
}
