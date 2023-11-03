// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Models;
using AutomationSamples.Shared.Services;
using BlazorFocused.Automation.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PowerShellSample;

public class Startup : AutomationStartup
{
    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["ReportOptions:Disclaimer"] = "Disclaimer: This is not real data, it is only for test output and demonstration",
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReportGenerator, ReportGenerator>();

        services
            .AddOptions<ReportOptions>()
            .BindConfiguration(nameof(ReportOptions));
    }

    public override IDictionary<string, string> GenerateParameterConfigurationMapping() =>
        new Dictionary<string, string>()
        {
            ["--limit"] = "ReportOptions:MaxResults",
            ["--l"] = "ReportOptions:MaxResults",
            ["--type"] = "ReportOptions:ReportType",
            ["--t"] = "ReportOptions:ReportType",
        };
}
