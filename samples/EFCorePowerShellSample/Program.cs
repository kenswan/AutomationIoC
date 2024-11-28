// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.Runtime;
using EFCorePowerShellSample.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EFCorePowerShellSample;

public class Program: AutomationStartup
{
    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["ToDoOptions:Header"] = "EF Core & PowerShell Demonstration"
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        services.AddScoped<IToDoStorageAdapter, ToDoDbContext>();

        services.AddDbContext<ToDoDbContext>(dbContextOptionsBuilder =>
        {
            string databasePath = ToDoDbContextFactory.DatabasePath;
            dbContextOptionsBuilder.UseSqlite($"Data Source={databasePath}");
        });
    }
}
