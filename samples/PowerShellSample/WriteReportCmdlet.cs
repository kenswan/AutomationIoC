﻿// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Services;
using AutomationIoC.PowerShell;
using Microsoft.Extensions.Logging;
using System.Management.Automation;

namespace PowerShellSample;

[Cmdlet(VerbsCommunications.Write, "Report")]
public class WriteReportCmdlet : AutomationShell<Startup>
{
    [Alias("h")]
    [Parameter(Mandatory = false, HelpMessage = "Header showing on report")]
    public string Header { get; set; } = "Header not set";

    [AutomationDependency]
    protected IReportService ReportService { get; set; }

    [AutomationDependency]
    private readonly ILogger<WriteReportCmdlet> logger = default;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        logger.LogInformation("Beginning Report Processing");

        List<string> reportDetails = new()
        {
            ReportService.GenerateReportHeader(Header),
            ReportService.GenerateReportData(),
            ReportService.GenerateReportDisclaimer()
        };

        foreach (string reportDetail in reportDetails)
        {
            WriteInformation(reportDetail, Array.Empty<string>());

            WriteObject(reportDetail);
        }
    }
}
