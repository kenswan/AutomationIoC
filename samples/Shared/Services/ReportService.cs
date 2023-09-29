// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Models;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Text;
using System.Text.Json;

namespace AutomationSamples.Shared.Services;

public class ReportService : IReportService
{
    private readonly IReportGenerator reportGenerator;
    private readonly ReportOptions reportOptions;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ReportService(
        IReportGenerator reportGenerator,
        IOptions<ReportOptions> reportOptions)
    {
        this.reportGenerator = reportGenerator;

        this.reportOptions = reportOptions.Value ??
            throw new ArgumentNullException(nameof(reportOptions));

        jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
    }

    public string GenerateReportData()
    {
        bool reportCategoryExists = Enum.TryParse(reportOptions.ReportType, out ReportCategory reportCategory);

        if (!reportCategoryExists)
        {
            return "Report Type was not found";
        }

        int count = reportOptions.MaxResults ?? 10;

        IEnumerable records = reportCategory switch
        {
            ReportCategory.Company => reportGenerator.GenerateCompanyReport(count),

            ReportCategory.Database => reportGenerator.GenerateDatabaseReport(count),

            ReportCategory.Product => reportGenerator.GenerateProductReport(count),

            _ => reportGenerator.GenerateProductReport(count),

        };

        return JsonSerializer.Serialize(records, jsonSerializerOptions);
    }

    public string GenerateReportDisclaimer() => GenerateSectionText(reportOptions.Disclaimer ?? "End of Report");

    public string GenerateReportHeader(string headerText) => GenerateSectionText(headerText);

    private static string GenerateSectionText(string sectionText)
    {
        string divider = "\n***************************************************************\n";

        var reportBuilder = new StringBuilder();

        reportBuilder.AppendLine(divider);
        reportBuilder.AppendLine(sectionText);
        reportBuilder.AppendLine(divider);

        return reportBuilder.ToString();
    }
}
