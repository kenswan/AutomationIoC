// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace ConsolesSample.Services;

public interface IReportService
{
    string GenerateReportHeader(string headerText);

    string GenerateReportData();

    string GenerateReportDisclaimer();
}
