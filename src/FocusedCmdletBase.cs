using System.Management.Automation;

namespace PowerShellFocused
{
    public abstract class FocusedCmdletBase : PSCmdlet
    {
        public void RunInstance()
        {
            BeginProcessing();
            ProcessRecord();
            EndProcessing();
        }
    }
}
