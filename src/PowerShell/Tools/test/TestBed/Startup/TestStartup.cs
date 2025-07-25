// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Services;
using AutomationIoC.Runtime;

namespace AutomationIoC.PowerShell.Tools.Test.TestBed.Startup;

public class TestStartup : AutomationStartup
{
    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["testOptions:mode"] = "basic-test",
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }
    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) =>
        services.AddTransient<ITestService, TestService>();
}
