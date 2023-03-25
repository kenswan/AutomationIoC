// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.PSCmdlets.Integration.Startup;

public class TestPSCmdletStartup : IIoCStartup
{
    public IAutomationEnvironment AutomationEnvironment { get; set; }

    public void Configure(IConfigurationBuilder configurationBuilder) => throw new NotImplementedException();

    public void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) => throw new NotImplementedException();
}
