// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell;
using EFCorePowerShellSample.Context;
using System.Management.Automation;

namespace EFCorePowerShellSample.Cmdlets;

[Cmdlet(VerbsCommon.Get, "ToDos")]
public class GetAllToDosCmdlet : AutomationShell<Program>
{
    [AutomationDependency]
    protected IToDoStorageAdapter ToDoStorageAdapter { get; set; }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        var toDoItems = this.ToDoStorageAdapter.SelectAllToDos().ToList();

        toDoItems.ForEach(WriteObject);

        WriteObject("All ToDos have been retrieved. Count: " + toDoItems.Count);
    }
}
