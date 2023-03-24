// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace AutomationIoc.PSCmdlets.Integration.Commands;

[Cmdlet(VerbsDiagnostic.Test, "Variable")]
public sealed class TestVariableCommand : IoCVariable
{
    [Parameter(Mandatory = true)]
    [Alias("k")]
    public string Key { get; set; }

    [Parameter(Mandatory = true)]
    [Alias("v")]
    public string Value { get; set; }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        SetVariable(Key, Value);
    }
}
