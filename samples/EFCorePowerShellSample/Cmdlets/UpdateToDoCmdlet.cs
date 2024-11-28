// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell;
using EFCorePowerShellSample.Context;
using EFCorePowerShellSample.Models;
using System.Management.Automation;

namespace EFCorePowerShellSample.Cmdlets;

[Cmdlet(VerbsData.Update, "ToDo")]
public class UpdateToDoCmdlet : AutomationShellAsync<Program>
{
    [Parameter(Position = 0, Mandatory = true)]
    public Guid Id { get; set; }

    [Parameter(Position = 0, Mandatory = false)]
    public string? Title { get; set; }

    [Parameter(Position = 1, Mandatory = false)]
    public string? Description { get; set; }

    [Parameter(Position = 1, Mandatory = false)]
    public bool? IsCompleted { get; set; }

    [AutomationDependency]
    protected IToDoStorageAdapter toDoStorageAdapter { get; set; }

    protected override async Task ProcessRecordAsync()
    {
        ToDoEntity existingToDoItem = await this.toDoStorageAdapter.SelectToDoByIdAsync(this.Id);

        existingToDoItem.Title = this.Title ?? existingToDoItem.Title;
        existingToDoItem.Title = this.Description ?? existingToDoItem.Title;
        existingToDoItem.IsCompleted = this.IsCompleted ?? existingToDoItem.IsCompleted;

        ToDoEntity updatedToDoItem =
            await this.toDoStorageAdapter.UpdateToDoAsync(existingToDoItem);

        WriteObject(updatedToDoItem);
    }
}
