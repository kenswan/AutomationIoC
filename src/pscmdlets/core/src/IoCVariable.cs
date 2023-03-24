// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Session;
using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC.PSCmdlets;

public abstract class IoCVariable : PSCmdlet
{
    protected void SetVariable(string key, object value) =>
        AutomationIoCRuntime.SetEnvironment(new SessionStateProxy(SessionState), key, value);
}
