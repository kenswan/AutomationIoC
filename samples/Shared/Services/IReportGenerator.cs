// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Models;

namespace AutomationSamples.Shared.Services;

public interface IReportGenerator
{
    IEnumerable<Company> GenerateCompanyReport(int count);

    IEnumerable<Database> GenerateDatabaseReport(int count);

    IEnumerable<Product> GenerateProductReport(int count);
}
