using System.Management.Automation;

namespace AutomationIoC.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Uninstall, "Dependencies")]
    public class TestTeardown : FocusedTeardown
    { }
}