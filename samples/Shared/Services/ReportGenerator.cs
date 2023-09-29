// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Models;
using Bogus;

namespace AutomationSamples.Shared.Services;

public class ReportGenerator : IReportGenerator
{
    public IEnumerable<Company> GenerateCompanyReport(int count) =>
        Initialize<Company>()
        .RuleFor(company => company.Id, fake => fake.Random.Guid().ToString())
        .RuleFor(company => company.CompanyName, fake => fake.Company.CompanyName())
        .RuleFor(company => company.Description, fake => fake.Company.Bs())
        .RuleFor(company => company.Slogan, fake => fake.Company.CatchPhrase())
        .Generate(count);

    public IEnumerable<Models.Database> GenerateDatabaseReport(int count) =>
        Initialize<Models.Database>()
        .RuleFor(database => database.Id, fake => fake.Random.Guid().ToString())
        .RuleFor(database => database.Name, fake => fake.Database.Column())
        .RuleFor(database => database.Engine, fake => fake.Database.Engine())
        .RuleFor(database => database.Collation, fake => fake.Database.Column())
        .RuleFor(database => database.Columns, _ => GenerateColumns())
        .Generate(count);

    public IEnumerable<Product> GenerateProductReport(int count) =>
        Initialize<Product>()
        .RuleFor(product => product.ProductName, fake => fake.Commerce.ProductName())
        .RuleFor(product => product.Price, fake => fake.Finance.Amount())
        .Generate(count);

    private static Faker<T> Initialize<T>() where T : RecordEntity =>
        new Faker<T>()
        .RuleFor(record => record.Id, fake => fake.Random.Guid().ToString());

    private static IList<string> GenerateColumns() =>
        new Faker().Make<string>(new Faker().Random.Int(5, 10), () => new Faker().Database.Column());
}
