using System.Management.Automation;

namespace AutomationIoC.Runtime.Session
{
    internal interface ISessionState
    {
        List<string> Applications { get; }

        DriveManagementIntrinsics Drive { get; }

        CommandInvocationIntrinsics InvokeCommand { get; }

        ProviderIntrinsics InvokeProvider { get; }

        PSLanguageMode LanguageMode { get; }

        PSModuleInfo Module { get; }

        PathIntrinsics Path { get; }

        CmdletProviderManagementIntrinsics Provider { get; }

        PSVariableIntrinsics PSVariable { get; }

        List<string> Scripts { get; }

        bool UseFullLanguageModeInDebugger { get; }
    }
}
