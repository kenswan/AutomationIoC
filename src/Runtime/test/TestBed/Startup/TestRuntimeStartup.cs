// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorFocused.Automation.Runtime.Test.TestBed.Services;

namespace BlazorFocused.Automation.Runtime.Test.TestBed.Startup;

public class TestRuntimeStartup : AutomationStartup
{
    public const string CONFIGURATION_KEY = "TestOptions";
    public const string CONFIGURATION_VALUE = "Basic-Test";

    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            [CONFIGURATION_KEY] = CONFIGURATION_VALUE,
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) =>
        services
            .AddTransient<ITestRuntimeService, TestRuntimePropertyService>()
            .AddTransient<ITestRuntimeService, TestRuntimeFieldService>()
            .AddTransient<ITestRuntimeInternalServiceOne, TestRuntimeInternalServiceOne>()
            .AddTransient<ITestRuntimeInternalServiceTwo, TestRuntimeInternalServiceTwo>();
}
