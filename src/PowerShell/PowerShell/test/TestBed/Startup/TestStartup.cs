// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorFocused.Automation.PowerShell.Test.Services;
using BlazorFocused.Automation.Runtime;

namespace BlazorFocused.Automation.PowerShell.Test.TestBed.Startup;

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
