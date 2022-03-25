using System.Management.Automation;

namespace AutomationIoC.Runtime.Session
{
    internal class SessionStateProxy : ISessionState
    {
        private readonly SessionState sessionState;

        public SessionStateProxy(SessionState sessionState)
        {
            this.sessionState = sessionState;
        }

        public List<string> Applications => sessionState.Applications;

        public DriveManagementIntrinsics Drive => sessionState.Drive;

        public CommandInvocationIntrinsics InvokeCommand => sessionState.InvokeCommand;

        public ProviderIntrinsics InvokeProvider => sessionState.InvokeProvider;

        public PSLanguageMode LanguageMode => sessionState.LanguageMode;

        public PSModuleInfo Module => sessionState.Module;

        public PathIntrinsics Path => sessionState.Path;

        public CmdletProviderManagementIntrinsics Provider => sessionState.Provider;

        public PSVariableIntrinsics PSVariable => sessionState.PSVariable;

        public List<string> Scripts => sessionState.Scripts;

        public bool UseFullLanguageModeInDebugger => sessionState.UseFullLanguageModeInDebugger;
    }
}
