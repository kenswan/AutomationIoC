// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace AutomationIoC.PowerShell.Session;

internal interface IPowerShellSessionState
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
