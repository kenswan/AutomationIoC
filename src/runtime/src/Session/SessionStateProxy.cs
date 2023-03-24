// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;
using Runspace = System.Management.Automation.Runspaces;

namespace AutomationIoC.Runtime.Session;

internal class SessionStateProxy : ISessionState
{
    private readonly SessionState sessionState;
    private readonly Runspace.SessionStateProxy sessionStateProxy;

    public SessionStateProxy(SessionState sessionState)
    {
        this.sessionState = sessionState;
    }

    public SessionStateProxy(Runspace.SessionStateProxy sessionStateProxy)
    {
        this.sessionStateProxy = sessionStateProxy;
    }

    public List<string> Applications => sessionState?.Applications ?? sessionStateProxy.Applications;

    public DriveManagementIntrinsics Drive => sessionState?.Drive ?? sessionStateProxy.Drive;

    public CommandInvocationIntrinsics InvokeCommand => sessionState?.InvokeCommand ?? sessionStateProxy.InvokeCommand;

    public ProviderIntrinsics InvokeProvider => sessionState?.InvokeProvider ?? sessionStateProxy.InvokeProvider;

    public PSLanguageMode LanguageMode => sessionState?.LanguageMode ?? sessionStateProxy.LanguageMode;

    public PSModuleInfo Module => sessionState?.Module ?? sessionStateProxy.Module;

    public PathIntrinsics Path => sessionState?.Path ?? sessionStateProxy.Path;

    public CmdletProviderManagementIntrinsics Provider => sessionState?.Provider ?? sessionStateProxy.Provider;

    public PSVariableIntrinsics PSVariable => sessionState?.PSVariable ?? sessionStateProxy.PSVariable;

    public List<string> Scripts => sessionState?.Scripts ?? sessionStateProxy.Scripts;

    public bool UseFullLanguageModeInDebugger => sessionState?.UseFullLanguageModeInDebugger ?? false;
}
