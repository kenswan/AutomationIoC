using AutomationIoC.Runtime;
using AutomationIoC.Tools.Context;
using System.Management.Automation;

namespace AutomationIoC.Tools
{
    public static class AutomationSandbox
    {
        public static IAutomationContext<TCommand, TStartup> CreateContext<TCommand, TStartup>()
            where TCommand : PSCmdlet
            where TStartup : IIoCStartup, new() =>
                new AutomationContext<TCommand, TStartup>();
    }
}
