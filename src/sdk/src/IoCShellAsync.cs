using AutomationIoC.Runtime;

namespace AutomationIoC;

public abstract class IoCShellAsync<TStartup> : IoCShell<TStartup> where TStartup : IIoCStartup, new()
{
    protected abstract Task ProcessRecordAsync();

    protected sealed override void ProcessRecord()
    {
        ProcessRecordAsync().Wait();
    }
}
