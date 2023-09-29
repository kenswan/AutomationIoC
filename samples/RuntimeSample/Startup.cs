// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Models;
using AutomationSamples.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RuntimeSample.Services;
using BlazorFocused.Automation.Runtime;

namespace RuntimeSample;

public class Startup : IAutomationStartup
{
    public void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["ReportOptions:Disclaimer"] =
                "Disclaimer: This is not real data, it is only for test output and demonstration",
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    public void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReportGenerator, ReportGenerator>();
        services.AddScoped<IReportOrchestrator, ReportOrchestratorWithConstructor>();

        services
            .AddOptions<ReportOptions>()
            .BindConfiguration(nameof(ReportOptions));
    }

    public IDictionary<string, string> GenerateParameterConfigurationMapping() =>
        new Dictionary<string, string>()
        {
            ["--limit"] = "ReportOptions:MaxResults",
            ["--l"] = "ReportOptions:MaxResults",
            ["--type"] = "ReportOptions:ReportType",
            ["--t"] = "ReportOptions:ReportType",
        };

    public string[] GenerateParameters() => Environment.GetCommandLineArgs();
}
