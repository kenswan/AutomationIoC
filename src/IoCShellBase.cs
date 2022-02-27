using System.Management.Automation;

namespace AutomationIoC
{
    public abstract class IoCShellBase : PSCmdlet
    {
        public void RunInstance()
        {
            BeginProcessing();
            ProcessRecord();
            EndProcessing();
        }
    }
}
