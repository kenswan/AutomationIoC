using AutomationIoC.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AutomationIoC.Tools.Context
{
    internal class AutomationContext<TCommand, TStartup> : IAutomationContext<TCommand, TStartup>
        where TCommand : IoCShell<TStartup>
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
            IServiceCollection serviceCollection = AutomationIoCRuntime.ExportRuntimeDependencies();

            buildServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            PSVariable serviceProviderVariable =
                    new(typeof(TStartup).Name, serviceProvider, ScopedItemOptions.ReadOnly);

            powerShellSession.Runspace.SessionStateProxy.PSVariable.Set(serviceProviderVariable);
        }

        public ICollection<PSObject> RunCommand() => powerShellSession.Invoke();

        private static string GetCommandName()
        {
            CmdletAttribute cmdletAttribute = 
                Attribute.GetCustomAttribute(typeof(TCommand), typeof(CmdletAttribute)) as CmdletAttribute;

            if(cmdletAttribute is null)
                throw new ArgumentException("CmdletAttribute not found on class", nameof(cmdletAttribute));

            return $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";
        }
    }
}
