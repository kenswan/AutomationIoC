using AutomationIoC.Runtime;

namespace AutomationIoC.Tools
{
    public class TestAutomationShell<TShell, TStartup> : IoCShell<TStartup>
            where TShell : IoCShell<TStartup>, new ()
            where TStartup : IIoCStartup, new ()
    {
        private readonly TShell shell;

        public TestAutomationShell(TShell shell)
        {
            this.shell = shell;
        }

        protected sealed override void BeginProcessing()
        {
        }

        public void Execute()
        {
            shell.RunInstance();
        }
    }
}
