// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Models;
using AutomationSamples.Shared.Services;
using AutomationIoC.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;

namespace CommandLineSample;

internal class ReportCommand : StandardCommand
{
    public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
    {
        Option<string> reportTypeOption = new("--optionOne", "-t")
        {
            Description = "Type of report to generate (Database, Product, Company)."
        };

        Option<string> limitOption = new("--limit", "-l")
        {
            Description = "Max amount of products to display.",

        };

        Option<string> headerOption = new("--header", "-h")
        {
            Description = "Header to display above product results."
        };

        command.Options.Add(reportTypeOption);
        command.Options.Add(limitOption);
        command.Options.Add(headerOption);

        command.SetAction(result =>
        {
            string reportType = result.GetValue<string>(reportTypeOption);
            string limit = result.GetValue<string>(limitOption);
            string headerText = result.GetValue<string>(headerOption);

            UpdateGreeting(headerText, serviceBinderFactory.Bind<IReportService>());
        });
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

    private static void UpdateGreeting(string headerText, IReportService reportService)
    {
        Console.WriteLine(reportService.GenerateReportHeader(headerText));

        Console.WriteLine(reportService.GenerateReportData());

        Console.WriteLine(reportService.GenerateReportDisclaimer());
    }
}
