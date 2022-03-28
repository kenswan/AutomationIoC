using AutomationIoC.Runtime.Session;

namespace AutomationIoC.Runtime.Environment
{
    internal class AutomationEnvironment : IAutomationEnvironment
    {
        private readonly ISessionState sessionState;

        public AutomationEnvironment(ISessionState sessionState)
        {
            this.sessionState = sessionState;
        }

        public T GetVariable<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetVariable<T>(string key, out T value)
        {
            throw new NotImplementedException();
        }
    }
}
