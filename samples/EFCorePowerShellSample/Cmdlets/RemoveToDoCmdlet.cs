// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell;
using EFCorePowerShellSample.Context;
using System.Management.Automation;

namespace EFCorePowerShellSample.Cmdlets;

[Cmdlet(VerbsCommon.Remove, "ToDo")]
public class RemoveToDoCmdlet : AutomationShellAsync<Program>
{
    [Parameter(Position = 0, Mandatory = true)]
    public Guid Id { get; set; }

    [AutomationDependency]
    protected IToDoStorageAdapter ToDoStorageAdapter { get; set; }

    protected override async Task ProcessRecordAsync()
    {

        bool isDeleted =
            await this.ToDoStorageAdapter.DeleteToDoAsync(this.Id).ConfigureAwait(false);

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
