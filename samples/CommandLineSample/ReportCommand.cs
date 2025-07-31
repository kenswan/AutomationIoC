// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Services;
using AutomationIoC.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace CommandLineSample;

internal class ReportCommand : IAutomationCommand
{
    public void Initialize(AutomationCommand command)
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

        command.SetAction((result, automationContext) =>
        {
            string reportType = result.GetValue<string>(reportTypeOption);
            string limit = result.GetValue<string>(limitOption);
            string headerText = result.GetValue<string>(headerOption);

            UpdateGreeting(headerText, automationContext.ServiceProvider.GetService<IReportService>());
        });
    }

    private static void UpdateGreeting(string headerText, IReportService reportService)
    {
        Console.WriteLine(reportService.GenerateReportHeader(headerText));

        Console.WriteLine(reportService.GenerateReportData());

        Console.WriteLine(reportService.GenerateReportDisclaimer());
    }
}
