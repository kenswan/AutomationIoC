using AutomationIoC.Runtime;
using System.Management.Automation;

namespace AutomationIoC
{
    public abstract class IoCShell<TStartup> : PSCmdlet where TStartup : IIoCStartup, new()
    {
        internal string CommandName { get { return MyInvocation.InvocationName; } }
        internal bool ShouldInitializeContext { get; set; } = true;

        protected override void BeginProcessing()
        {
            WriteVerbose($"Command {CommandName} Started");

            base.BeginProcessing();

            if (ShouldInitializeContext)
            {
                var dependencyContext = new DependencyContext<AutomationDependencyAttribute, TStartup>
                {
                    Instance = this,
                    SessionState = SessionState
                };

                AutomationIoCRuntime.BindContext(dependencyContext);
            }

            WriteVerbose($"{CommandName} Context Initialized");
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            WriteVerbose($"{CommandName} process completed");
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
