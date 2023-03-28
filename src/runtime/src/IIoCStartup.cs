// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime;

public interface IIoCStartup
{
    IAutomationEnvironment AutomationEnvironment { get; set; }

    void Configure(IConfigurationBuilder configurationBuilder);

    void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services);
}
