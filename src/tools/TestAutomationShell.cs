﻿using AutomationIoC.Runtime;

namespace AutomationIoC.Tools
{
    public class TestAutomationShell<TShell, TStartup>
            where TShell : IoCShell<TStartup>, new ()
            where TStartup : IIoCStartup, new ()
    {
        private readonly TShell shell;

        public TestAutomationShell(TShell shell)
        {
            this.shell = shell;
        }
        public void Invoke()
        {
            shell.RunInstance();
        }
    }
}