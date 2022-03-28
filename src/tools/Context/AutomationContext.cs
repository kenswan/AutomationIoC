using AutomationIoC.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AutomationIoC.Tools.Context
{
    internal class AutomationContext<TCommand, TStartup> : IAutomationContext<TCommand, TStartup>
        where TCommand : PSCmdlet
        where TStartup : IIoCStartup, new()
    {
        private readonly PowerShell powerShellSession;

        public AutomationContext()
        {
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { typeof(TCommand).Assembly.Location });

            Runspace runspace = RunspaceFactory.CreateRunspace(initial);
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

        public void ConfigureServices(Action<IServiceCollection> buildServices)
        {
            IServiceCollection services = new ServiceCollection();

            buildServices(services);

            AutomationIoCRuntime.BuildServices<TStartup>(powerShellSession.Runspace.SessionStateProxy, services);
        }

        public ICollection<PSObject> RunCommand() => powerShellSession.Invoke();

        private static string GetCommandName()
        {
            if (Attribute.GetCustomAttribute(typeof(TCommand), typeof(CmdletAttribute)) is not CmdletAttribute cmdletAttribute)
                throw new ArgumentException("CmdletAttribute not found on class", nameof(cmdletAttribute));

            return $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";
        }

        public void SetVariable(string name, object value)
        {
            AutomationIoCRuntime.SetEnvironment(powerShellSession.Runspace.SessionStateProxy, name, value);
        }
    }
}
