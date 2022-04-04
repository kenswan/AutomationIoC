using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC.SDK
{
    public abstract class IoCVariable : PSCmdlet
    {
        protected void SetVariable(string key, object value)
        {
            AutomationIoCRuntime.SetEnvironment(SessionState, key, value);
        }
    }
}
