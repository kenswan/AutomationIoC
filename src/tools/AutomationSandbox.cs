using AutomationIoC.Runtime;
using AutomationIoC.Tools.Command;
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

        public static IAutomationCommand<TCommand> CreateCommand<TCommand>()
            where TCommand : PSCmdlet, new() =>
                new AutomationCommand<TCommand>();

        public static IOpenCommandSession StartSession() =>
                new OpenCommandSession();
    }
}
