using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AutomationIoC.Tools
{
    public static class TestAutomationShell
    {
        public static ICollection<PSObject> RunCommand<T>(
            string commandName,
            Action<PSCommand> buildCommand = null)
        {
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { typeof(T).Assembly.Location });

            Runspace runspace = RunspaceFactory.CreateRunspace(initial);
            runspace.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = runspace;
            
            var command = ps.Commands.AddCommand(commandName);

            if (buildCommand is not null)
                buildCommand(command);

            return ps.Invoke();
        }

        public static ICollection<PSObject> RunCommand(Action<PSCommand> buildCommands)
        {
            InitialSessionState initial = InitialSessionState.CreateDefault();
            Runspace runspace = RunspaceFactory.CreateRunspace(initial);
            runspace.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = runspace;

            buildCommands(ps.Commands);

            return ps.Invoke();
        }
    }
}
