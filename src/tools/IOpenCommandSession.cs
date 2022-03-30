using System.Management.Automation;

namespace AutomationIoC.Tools
{
    public interface IOpenCommandSession
    {
        public void ImportModule(params string[] modulePaths);

        public ICollection<PSObject> RunCommand(string commandName, Action<PSCommand> buildCommand);
    }
}
