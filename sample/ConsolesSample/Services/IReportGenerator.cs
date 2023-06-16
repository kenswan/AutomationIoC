// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using ConsolesSample.Models;

namespace ConsolesSample.Services;

public interface IReportGenerator
{
    IEnumerable<Company> GenerateCompanyReport(int count);

    IEnumerable<Database> GenerateDatabaseReport(int count);

    IEnumerable<Product> GenerateProductReport(int count);
}
