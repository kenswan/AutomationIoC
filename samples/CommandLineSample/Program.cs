// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine;
using AutomationSamples.Shared.Models;
using AutomationSamples.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommandLineSample;

internal class Program
{
    public static void Main(string[] args)
    {
        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("Sample CommandLine Example", args)
                .AddCommand<ReportCommand>("report")
                .Configure((hostBuilderContext, configurationBuilder) =>
                {
                    var appSettings = new Dictionary<string, string>()
                    {
                        ["ReportOptions:Disclaimer"] =
                            "Disclaimer: This is not real data, it is only for test output and demonstration",
                    };

                    configurationBuilder.AddInMemoryCollection(appSettings);
                })
                .ConfigureServices((hostContextBuilder, services) =>
                {
                    services.AddScoped<IReportService, ReportService>();
                    services.AddScoped<IReportGenerator, ReportGenerator>();

                    services
                        .AddOptions<ReportOptions>()
                        .BindConfiguration(nameof(ReportOptions));
                })
                .WithConfigurationMapping(new Dictionary<string, string>()
                {
                    ["--limit"] = "ReportOptions:MaxResults",
                    ["--l"] = "ReportOptions:MaxResults",
                    ["--type"] = "ReportOptions:ReportType",
                    ["--t"] = "ReportOptions:ReportType",
                });

        IAutomationConsole console = builder.Build();

        console.Run();
    }
}

