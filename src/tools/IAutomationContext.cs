using AutomationIoC.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.Tools
{
    public interface IAutomationContext<TCommand, TStartup> : IAutomationCommand<TCommand>
        where TCommand : PSCmdlet
        where TStartup : IIoCStartup, new()
    {
        void ConfigureServices(Action<IServiceCollection> buildServices);
    }
}
