using AutomationIoC.Runtime;
using AutomationIoC.Tools.Command;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.Tools.Context
{
    internal class AutomationContext<TCommand, TStartup> : AutomationCommand<TCommand>, IAutomationContext<TCommand, TStartup>
        where TCommand : PSCmdlet
        where TStartup : IIoCStartup, new()
    {
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
    }
}
