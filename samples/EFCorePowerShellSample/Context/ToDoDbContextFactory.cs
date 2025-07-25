// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCorePowerShellSample.Context;

public class ToDoDbContextFactory : IDesignTimeDbContextFactory<ToDoDbContext>
{
    public static string DatabasePath => GetDatabasePath();

    private static string GetDatabasePath()
    {
        const string directoryName = "AutomationIoCAutomation";
        const string databaseName = "EFCorePowerShellSample.db";

        string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string directory = Path.Combine(basePath, directoryName);

        if (Directory.Exists(directory) is false)
        {
            Directory.CreateDirectory(directory);
        }

        return Path.Combine(directory, databaseName);
    }

    public ToDoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ToDoDbContext>();
        optionsBuilder.UseSqlite($"Data Source={DatabasePath}");

        return new ToDoDbContext(optionsBuilder.Options);
    }
}
