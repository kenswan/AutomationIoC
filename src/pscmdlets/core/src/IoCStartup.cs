// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.PSCmdlets;

public abstract class IoCStartup : IIoCStartup
{
    public IAutomationEnvironment AutomationEnvironment { get; set; }

    public abstract void Configure(IConfigurationBuilder configurationBuilder);

    public abstract void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services);
}
