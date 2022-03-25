using AutomationIoC.Runtime;

namespace AutomationIoC.Tools
{
    public class TestAutomationCmdlet<TShell, TStartup>
            where TShell : IoCShell<TStartup>, new ()
            where TStartup : IIoCStartup, new ()
    {
        private readonly TShell shell;

        public TestAutomationCmdlet(TShell shell)
        {
            this.shell = shell;
        }

        public void Execute()
        {
            shell.RunInstance();
        }
    }
}
