// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoc.PSCmdlets;

public abstract class IoCStartup : IIoCStartup
{
    public IConfiguration Configuration { get; set; }
    public IAutomationEnvironment AutomationEnvironment { get; set; }

    public abstract void Configure(IConfigurationBuilder configurationBuilder);

    public abstract void ConfigureServices(IServiceCollection services);
}
