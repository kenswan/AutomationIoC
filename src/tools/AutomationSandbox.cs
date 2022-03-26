using AutomationIoC.Runtime;
using AutomationIoC.Tools.Context;

namespace AutomationIoC.Tools
{
    public static class AutomationSandbox
    {
        public static IAutomationContext<TCommand, TStartup> CreateContext<TCommand, TStartup>()
            where TCommand : IoCShell<TStartup>
            where TStartup : IIoCStartup, new() =>
                new AutomationContext<TCommand, TStartup>();
    }
}
