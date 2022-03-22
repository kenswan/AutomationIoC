using AutomationIoC.Context;
using System.Management.Automation;

namespace AutomationIoC
{
    public abstract class IoCShellBase : PSCmdlet
    {
        internal IAutomationContext Context { get; set; }
        internal string CommandName { get { return MyInvocation.InvocationName; } }

        protected sealed override void BeginProcessing()
        {
            ;
            WriteVerbose($"Command {CommandName} Started");

            base.BeginProcessing();

            if (Context is null)
                Context = new AutomationContext(SessionState);

            Context.InitializeCurrentInstance(this);

            WriteVerbose($"{CommandName} Context Initialized");
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();

            WriteVerbose($"{CommandName} operation completed");
        }

        internal void RunInstance()
        {
            BeginProcessing();
            ProcessRecord();
            EndProcessing();
        }
    }
}
