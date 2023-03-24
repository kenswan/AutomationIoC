// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;

namespace AutomationIoC.PSCmdlets;

public abstract class IoCShellAsync<TStartup> : IoCShell<TStartup> where TStartup : IIoCStartup, new()
{
    protected abstract Task ProcessRecordAsync();

    protected sealed override void ProcessRecord() => ProcessRecordAsync().Wait();
}
