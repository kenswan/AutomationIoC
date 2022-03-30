using AutomationIoC.Runtime;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AutomationIoC.Tools.Command
{
    public class AutomationCommand<TCommand> : IAutomationCommand<TCommand>
        where TCommand : PSCmdlet
    {
        protected readonly PowerShell powerShellSession;
        protected readonly Runspace runspace;

        public AutomationCommand()
        {
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { typeof(TCommand).Assembly.Location });

            runspace = RunspaceFactory.CreateRunspace(initial);
            runspace.Open();

            powerShellSession = PowerShell.Create();
            powerShellSession.Runspace = runspace;

            var commandName = GetCommandName();

            powerShellSession.Commands.AddCommand(commandName);
        }

        public void ConfigureParameters(Action<PSCommand> buildCommand)
        {
            buildCommand(powerShellSession.Commands);
        }

        public ICollection<PSObject> RunCommand() => powerShellSession.Invoke();

        public void SetVariable(string name, object value)
        {
            AutomationIoCRuntime.SetEnvironment(powerShellSession.Runspace.SessionStateProxy, name, value);
        }

        private static string GetCommandName()
        {
            if (Attribute.GetCustomAttribute(typeof(TCommand), typeof(CmdletAttribute)) is not CmdletAttribute cmdletAttribute)
                throw new ArgumentException("CmdletAttribute not found on class", nameof(cmdletAttribute));

            return $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";
        }

        ~AutomationCommand()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (runspace is not null)
            {
                runspace.Close();
                runspace.Dispose();
            }

            if (powerShellSession is not null)
            {
                powerShellSession.Stop();
                powerShellSession.Dispose();
            }
        }
    }
}
