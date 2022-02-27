using System.Management.Automation;

namespace AutomationIoC
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
