using System.Management.Automation;

namespace AutomationIoC.Tools.Command
{
    internal class AutomationCommand<TCommand> : NativeCommand, IAutomationCommand<TCommand>
        where TCommand : PSCmdlet
    {
        protected readonly string commandName;

        public AutomationCommand() : base(typeof(TCommand).Assembly.Location)
        {
            commandName = GetCommandName();
        }

        public void SetVariable(string name, object value)
        {
            RunExternalCommand("Set-Variable", command =>
                command
                    .AddParameter("Name", name)
                    .AddParameter("Value", value));
        }

        public ICollection<PSObject> RunCommand(Action<PSCommand> buildCommand = null)
        {
            return InvokeCommand<PSObject>(commandName, buildCommand);
        }

        public ICollection<T> RunCommand<T>(Action<PSCommand> buildCommand = null)
        {
            return InvokeCommand<T>(commandName, buildCommand);
        }

        public ICollection<PSObject> RunExternalCommand(string name, Action<PSCommand> buildCommand = null)
        {
            return InvokeCommand<PSObject>(name, buildCommand);
        }

        public ICollection<T> RunExternalCommand<T>(string name, Action<PSCommand> buildCommand = null)
        {
            return InvokeCommand<T>(name, buildCommand);
        }

        private static string GetCommandName()
        {
            if (Attribute.GetCustomAttribute(typeof(TCommand), typeof(CmdletAttribute)) is not CmdletAttribute cmdletAttribute)
                throw new ArgumentException("CmdletAttribute not found on class", nameof(cmdletAttribute));

            return $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";
        }
    }
}
