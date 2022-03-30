using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AutomationIoC.Tools.Command
{
    internal class OpenCommandSession : IOpenCommandSession, IDisposable
    {
        protected readonly PowerShell powerShellSession;
        protected readonly Runspace runspace;

        public OpenCommandSession()
        {
            InitialSessionState initial = InitialSessionState.CreateDefault();

            runspace = RunspaceFactory.CreateRunspace(initial);
            runspace.Open();

            powerShellSession = PowerShell.Create();
            powerShellSession.Runspace = runspace;
        }

        public void ImportModule(params string[] modulePaths)
        {
            powerShellSession.Runspace.InitialSessionState.ImportPSModule(modulePaths);
        }

        public ICollection<PSObject> RunCommand(string commandName, Action<PSCommand> buildCommand)
        {
            powerShellSession.Commands.Clear();

            buildCommand(powerShellSession.Commands.AddCommand(commandName));

            return powerShellSession.Invoke();
        }

        ~OpenCommandSession()
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
