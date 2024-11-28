// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using EFCorePowerShellSample.Context;
using EFCorePowerShellSample.Models;
using Microsoft.EntityFrameworkCore;
using System.Management.Automation;

namespace EFCorePowerShellSample.Cmdlets;

[Cmdlet(VerbsDiagnostic.Test, "DbContext")]
public class TestContextCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        // Use the centralized database path from the factory
        string databasePath = ToDoDbContextFactory.DatabasePath;

        DbContextOptions<ToDoDbContext> options = new DbContextOptionsBuilder<ToDoDbContext>()
            .UseSqlite($"Data Source={databasePath}")
            .Options;

        using var context = new ToDoDbContext(options);

        var toDos = context.ToDos.ToList();

        foreach (ToDoEntity toDo in toDos)
        {
            WriteObject(toDo);
        }

        if (!toDos.Any())
        {
            var todo = new ToDoEntity
            {
                Title = "Test ToDo",
                Description = "Test Description",
                IsCompleted = false
            };

            context.ToDos.Add(todo);
            context.SaveChanges();
        }

        WriteObject($"Total ToDos: {toDos.Count}");
    }
}
