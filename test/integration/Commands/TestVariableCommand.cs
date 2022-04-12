using AutomationIoC.Runtime;
using AutomationIoC.SDK;
using System.Management.Automation;

namespace AutomationIoC.Integration.Commands;

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
