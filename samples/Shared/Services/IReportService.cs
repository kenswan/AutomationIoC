// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationSamples.Shared.Services;

public interface IReportService
{
    string GenerateReportHeader(string headerText);

    string GenerateReportData();

    string GenerateReportDisclaimer();
}
