using AutomationIoC.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.Tools
{
    public interface IAutomationContext<TCommand, TStartup>
        where TCommand : IoCShell<TStartup>
        where TStartup : IIoCStartup, new()
    {
        void ConfigureServices(Action<IServiceCollection> buildServices);

        void ConfigureParameters(Action<PSCommand> buildCommand);

        ICollection<PSObject> RunCommand();
    }
}
