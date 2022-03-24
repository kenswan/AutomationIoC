using AutomationIoC.Runtime;

namespace AutomationIoC.Tools
{
    internal class TestAutomationContext
    {
        public TestAutomationShell<TShell, TStartup> CreateInstance<TShell, TStartup>(params object[] dependencies)
            where TShell : IoCShell<TStartup>, new()
            where TStartup : IIoCStartup, new()
        {
            var shell = new TShell();

            // load mock dependencies through inflection

            return new TestAutomationShell<TShell, TStartup>(shell);
        }
    }
}
