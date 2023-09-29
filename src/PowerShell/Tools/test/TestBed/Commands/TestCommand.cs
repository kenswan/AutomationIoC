// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Commands;

[Cmdlet(VerbsDiagnostic.Test, "Command")]
public class TestCommand : PSCmdlet
{
    [Parameter(Mandatory = true)]
    public string Test;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        WriteObject(Test);
    }
}
