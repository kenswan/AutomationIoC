// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell;
using EFCorePowerShellSample.Context;
using EFCorePowerShellSample.Models;
using System.Management.Automation;

namespace EFCorePowerShellSample.Cmdlets;

[Cmdlet(VerbsCommon.Add, "ToDo")]
public class AddToDoCmdlet : AutomationShellAsync<Program>
{
    [Parameter(Position = 0, Mandatory = true)]
    public string Title { get; set; }

    [Parameter(Position = 1, Mandatory = true)]
    public string Description { get; set; }

    [AutomationDependency]
    protected IToDoStorageAdapter ToDoStorageAdapter { get; set; }

    protected override async Task ProcessRecordAsync()
    {
        var newToDoItem = new ToDoEntity
        {
            Title = this.Title,
            Description = this.Description,
            IsCompleted = false
        };

        ToDoEntity toDoItem =
            await this.ToDoStorageAdapter.InsertToDoAsync(newToDoItem).ConfigureAwait(false);

        WriteObject(toDoItem);
    }
}
