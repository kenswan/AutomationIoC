// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Consoles;
using ConsolesSample.Models;
using ConsolesSample.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace ConsolesSample.Commands;

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
            serviceBinderFactory.Create<IReportService>());
    }

    public override Action<IConfigurationBuilder> ConfigurationBuilder => (configurationBuilder) =>
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["ReportOptions:Disclaimer"] = "Disclaimer: This is not real data, it is only for test output and demonstration",
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    };

    public override IDictionary<string, string> ConfigurationMapping =>
        new Dictionary<string, string>()
        {
            ["--limit"] = "ReportOptions:MaxResults",
            ["--l"] = "ReportOptions:MaxResults",
            ["--type"] = "ReportOptions:ReportType",
            ["--t"] = "ReportOptions:ReportType",
        };

    public override Action<IServiceCollection> Services => (serviceCollection) =>
    {
        serviceCollection.AddScoped<IReportService, ReportService>();
        serviceCollection.AddScoped<IReportGenerator, ReportGenerator>();

        serviceCollection
            .AddOptions<ReportOptions>()
            .BindConfiguration(nameof(ReportOptions));
    };

    private void UpdateGreeting(string headerText, IReportService reportService)
    {
        Console.WriteLine(reportService.GenerateReportHeader(headerText));

        Console.WriteLine(reportService.GenerateReportData());

        Console.WriteLine(reportService.GenerateReportDisclaimer());
    }
}
