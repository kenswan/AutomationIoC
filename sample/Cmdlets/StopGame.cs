using System.Management.Automation;

namespace PowerShellFocused.Sample.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Stop, "Game")]
    public class StopGame : FocusedTeardown
    { }
}
