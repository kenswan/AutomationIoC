// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.Runtime;
using System.Management.Automation;
using Runspace = System.Management.Automation.Runspaces;

namespace BlazorFocused.Automation.PowerShell.Session;

internal class AutomationSessionStateProxy : IPowerShellSessionState, ISessionStorage
{
    private readonly SessionState sessionState;
    private readonly Runspace.SessionStateProxy sessionStateProxy;

    public AutomationSessionStateProxy(SessionState sessionState)
    {
        this.sessionState = sessionState;
    }

    public AutomationSessionStateProxy(Runspace.SessionStateProxy sessionStateProxy)
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

    public T GetValue<T>(string key)
    {
        PSVariable psVariable = PSVariable.Get(key);

        return psVariable?.Value is not null ? (T)psVariable.Value : default;
    }

    public void SetValue<T>(string key, T value)
    {
        PSVariable serviceVariable =
                new(key, value, ScopedItemOptions.None);

        PSVariable.Set(serviceVariable);
    }
}
