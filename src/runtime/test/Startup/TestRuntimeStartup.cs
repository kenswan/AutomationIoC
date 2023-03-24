// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoc.Runtime.Services;
using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoc.Runtime.Startup;

public class TestRuntimeStartup : IIoCStartup
{
    public const string CONFIGURATION_KEY = "TestOptions";
    public const string CONFIGURATION_VALUE = "Basic-Test";

    public IConfiguration Configuration { get; set; }

    public IAutomationEnvironment AutomationEnvironment { get; set; }

    public void Configure(IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            [CONFIGURATION_KEY] = CONFIGURATION_VALUE,
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    public void ConfigureServices(IServiceCollection services) => services
            .AddTransient<ITestRuntimeService, TestRuntimePropertyService>()
            .AddTransient<ITestRuntimeService, TestRuntimeFieldService>()
            .AddTransient<ITestRuntimeInternalServiceOne, TestRuntimeInternalServiceOne>()
            .AddTransient<ITestRuntimeInternalServiceTwo, TestRuntimeInternalServiceTwo>();
}
