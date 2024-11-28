// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell;
using EFCorePowerShellSample.Context;
using System.Management.Automation;

namespace EFCorePowerShellSample.Cmdlets;

[Cmdlet(VerbsCommon.Get, "ToDos")]
public class GetAllToDosCmdlet : AutomationShell<Program>
{
    [AutomationDependency]
    protected IToDoStorageAdapter toDoStorageAdapter { get; set; }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        var toDoItems = this.toDoStorageAdapter.SelectAllToDos().ToList();

        toDoItems.ForEach(WriteObject);

        WriteObject("All ToDos have been retrieved. Count: " + toDoItems.Count);
    }
}
