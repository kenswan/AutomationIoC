// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell;
using EFCorePowerShellSample.Context;
using System.Management.Automation;

namespace EFCorePowerShellSample.Cmdlets;

[Cmdlet(VerbsCommon.Remove, "ToDo")]
public class RemoveToDoCmdlet : AutomationShellAsync<Program>
{
    [Parameter(Position = 0, Mandatory = true)]
    public Guid Id { get; set; }

    [AutomationDependency]
    protected IToDoStorageAdapter toDoStorageAdapter { get; set; }

    protected override async Task ProcessRecordAsync()
    {

        bool isDeleted =
            await this.toDoStorageAdapter.DeleteToDoAsync(this.Id);

        if (isDeleted)
        {
            WriteObject("ToDo item " + this.Id + " has been deleted");
        }
        else
        {
            WriteObject("ToDo item " + this.Id + " has not been deleted");
        }
    }
}
