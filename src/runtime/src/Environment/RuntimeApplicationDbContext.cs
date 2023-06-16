// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace AutomationIoC.Runtime.Environment;

internal class RuntimeApplicationDbContext : DbContext, IApplicationProvider
{
    public DbSet<RuntimeApplication> RuntimeApplications { get; set; }

    public DbSet<RuntimeConfiguration> RuntimeConfigurations { get; set; }

    public RuntimeApplicationDbContext(DbContextOptions<RuntimeApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string appDataFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        string automationFolderPath = Path.Join(appDataFolderPath, "AutomationIoC");

        bool automationFolderExists = Directory.Exists(automationFolderPath);
        if (!automationFolderExists)
        {
            Directory.CreateDirectory(automationFolderPath);
        }

        string databaseFilePath = Path.Join(automationFolderPath, "AutomationIoC.db");

        optionsBuilder.UseSqlite($"Data Source={databaseFilePath}");

        base.OnConfiguring(optionsBuilder);
    }
}
