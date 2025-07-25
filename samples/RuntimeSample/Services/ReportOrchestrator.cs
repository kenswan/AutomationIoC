// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Services;
using RuntimeSample.Attributes;

namespace RuntimeSample.Services;

public class ReportOrchestratorWithConstructor : IReportOrchestrator
{
    private readonly IReportService reportService;

    public ReportOrchestratorWithConstructor(
        IReportService reportService)
    {
        this.reportService = reportService;
    }

    public void CompileReport()
    {
        string header = "This used regular constructor dependency injection";

        Console.WriteLine(reportService.GenerateReportHeader(header));

        Console.WriteLine(reportService.GenerateReportData());

        Console.WriteLine(reportService.GenerateReportDisclaimer());
    }
}

public class ReportOrchestratorWithAttributes : IReportOrchestrator
{
    [ReportDependency]
    protected IReportService ReportService { get; set; }

    public void CompileReport()
    {
        string header = "This used regular constructor dependency injection";

        Console.WriteLine(ReportService.GenerateReportHeader(header));

        Console.WriteLine(ReportService.GenerateReportData());

        Console.WriteLine(ReportService.GenerateReportDisclaimer());
    }
}
