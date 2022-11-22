using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC;

public abstract class IoCVariable : PSCmdlet
{
    protected void SetVariable(string key, object value)
    {
        AutomationIoCRuntime.SetEnvironment(SessionState, key, value);
    }
}
