// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Models;
using AutomationSamples.Shared.Services;
using BlazorFocused.Automation.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;

namespace CommandLineSample;

internal class ReportCommand : StandardCommand
{
    public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
    {
        var reportTypeOption = new Option<string>(
            aliases: new[] { "--type", "-t" },
            description: "Type of report to generate (Database, Product, Company).");

        var limitOption = new Option<string>(
            aliases: new[] { "--limit", "-l" },
            description: "Max amount of products to display.");

        var headerOption = new Option<string>(
            aliases: new[] { "--header", "-h" },
            description: "Header to display above product results.");

        command.AddOption(reportTypeOption);
        command.AddOption(limitOption);
        command.AddOption(headerOption);

        command.SetHandler(UpdateGreeting,
            headerOption,
            serviceBinderFactory.Bind<IReportService>());
    }

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

    private void UpdateGreeting(string headerText, IReportService reportService)
    {
        Console.WriteLine(reportService.GenerateReportHeader(headerText));

        Console.WriteLine(reportService.GenerateReportData());

        Console.WriteLine(reportService.GenerateReportDisclaimer());
    }
}
