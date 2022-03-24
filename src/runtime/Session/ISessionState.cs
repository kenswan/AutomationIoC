using System.Management.Automation;

namespace AutomationIoC.Runtime.Session
{
    internal interface ISessionState
    {
        PSVariableIntrinsics PSVariable { get; }
    }
}
