// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC.PSCmdlets;

public abstract class IoCVariable : PSCmdlet
{
    protected void SetVariable(string key, object value) => AutomationIoCRuntime.SetEnvironment(SessionState, key, value);
}
