using System.Management.Automation;

namespace AutomationIoC.Tools
{
    public interface IAutomationCommand<TCommand> : IDisposable where TCommand : PSCmdlet
    {
        void ConfigureParameters(Action<PSCommand> buildCommand);

        ICollection<PSObject> RunCommand();

        void SetVariable(string name, object value);
    }
}
