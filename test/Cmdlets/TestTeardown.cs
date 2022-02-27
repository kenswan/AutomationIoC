using System.Management.Automation;

namespace PowerShellFocused.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Uninstall, "Dependencies")]
    public class TestTeardown : FocusedTeardown
    { }
}