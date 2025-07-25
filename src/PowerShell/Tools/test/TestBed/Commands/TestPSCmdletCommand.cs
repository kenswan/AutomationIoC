// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools.Test.TestBed.Commands;

[Cmdlet(VerbsDiagnostic.Test, "PSCmdlet")]
public class TestPSCmdletCommand : PSCmdlet
{
    [Parameter(Mandatory = true)]
    public string Test;

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        WriteObject(TransformOutput(Test));
    }

    public static string TransformOutput(string output) => $"This is processed: {output}";
}
