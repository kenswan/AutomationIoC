// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine;
using AutomationIoC.Runtime;
using AutomationSamples.Shared.Models;
using AutomationSamples.Shared.Services;
using CommandLineSample.Standalone;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Parsing;

// Configure Services
var commandAutomationContext = new AutomationContext();

commandAutomationContext.SetConfigure((hostBuilderContext, configurationBuilder) =>
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["ReportOptions:Disclaimer"] =
                "Disclaimer: This is not real data, it is only for test output and demonstration",
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    });

commandAutomationContext.SetConfigureServices((hostContextBuilder, services) =>
{
    services.AddScoped<IReportService, ReportService>();
    services.AddScoped<IReportGenerator, ReportGenerator>();

    services
        .AddOptions<ReportOptions>()
        .BindConfiguration(nameof(ReportOptions));
});

commandAutomationContext.SetConfigurationMapping(new Dictionary<string, string>()
{
    ["--limit"] = "ReportOptions:MaxResults",
    ["--l"] = "ReportOptions:MaxResults",
    ["--type"] = "ReportOptions:ReportType",
    ["--t"] = "ReportOptions:ReportType",
});

// Create Standalone Command
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

AutomationCommand rootCommand =
    AutomationConsole.CreateRootCommand(
        commandAutomationContext,
        "Sample CommandLine Example (with standalone commands)");

rootCommand.SetAction((result) =>
{
    Console.WriteLine("Nothing to see here, making this an empty root command for now");
});

// Manual Command Building Example
AutomationCommand subCommand = rootCommand.AddCommand("report", "Generate various reports");

subCommand.Options.Add(reportTypeOption);
subCommand.Options.Add(limitOption);
subCommand.Options.Add(headerOption);

subCommand.SetAction((result, automationContext) =>
{
    string reportType = result.GetValue<string>(reportTypeOption);
    string limit = result.GetValue<string>(limitOption);
    string headerText = result.GetValue<string>(headerOption);

    UpdateGreeting(headerText, automationContext.ServiceProvider.GetService<IReportService>());
});

// Build another Command with Initializer Example
AutomationCommand _ =
    rootCommand.AddCommand<ReportCommandInitializer>(
        "report-automated",
        "Generate various reports built by initializer");

// Execute Command
string[] commandLineArgs = [.. Environment.GetCommandLineArgs().Skip(1)];

ParseResult parseResult = rootCommand.Parse(commandLineArgs);

if (parseResult.Errors.Count == 0)
{
    parseResult.Invoke();
}
else
{
    foreach (ParseError error in parseResult.Errors)
    {
        Console.WriteLine(error.Message);
    }
}

return;

static void UpdateGreeting(string headerText, IReportService reportService)
{
    Console.WriteLine(reportService.GenerateReportHeader(headerText));

    Console.WriteLine(reportService.GenerateReportData());

    Console.WriteLine(reportService.GenerateReportDisclaimer());
}
