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
        private readonly Runspace runspace;
        public AutomationContext()
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

        /// <summary>
        /// This method will override original services set in <see cref="IIoCStartup" /> class
        /// </summary>
        /// <param name="buildServices">Configured services (usually mocks for testing)</param>
        public void ConfigureServices(Action<IServiceCollection> buildServices)
        {
            IServiceCollection services = new ServiceCollection();

            buildServices(services);

            AutomationIoCRuntime.BuildServices<TStartup>(powerShellSession.Runspace.SessionStateProxy, services);
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

        public void Dispose()
        {
            if(runspace is not null)
            {
                runspace.Close();
                runspace.Dispose();
            }

            if(powerShellSession is not null)
            {
                powerShellSession.Stop();
                powerShellSession.Dispose();
            }
        }
    }
}
