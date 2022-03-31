using System.Management.Automation;

namespace AutomationIoC.Tools
{
    public interface IAutomationCommand<TCommand> : IDisposable where TCommand : PSCmdlet
    {
        void ImportModule(string modulePath);

        ICollection<PSObject> RunCommand(Action<PSCommand> buildCommand = null);

        ICollection<T> RunCommand<T>(Action<PSCommand> buildCommand = null);
        
        ICollection<PSObject> RunExternalCommand(string name, Action<PSCommand> buildCommand = null);

        ICollection<T> RunExternalCommand<T>(string name, Action<PSCommand> buildCommand = null);

        void SetVariable(string name, object value);
    }
}
