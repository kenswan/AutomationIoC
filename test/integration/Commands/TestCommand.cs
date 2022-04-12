using System.Management.Automation;

namespace AutomationIoC.Integration.Commands;

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
