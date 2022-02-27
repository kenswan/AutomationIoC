using System.Management.Automation;

namespace AutomationIoC.Sample.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Stop, "Game")]
    public class StopGame : AutomationTeardown
    { }
}
