using System.Management.Automation;

namespace PowerShellFocused
{
    [Cmdlet(VerbsLifecycle.Uninstall, "Dependencies")]
    public class TestTeardown : FocusedTeardown
    { }
}