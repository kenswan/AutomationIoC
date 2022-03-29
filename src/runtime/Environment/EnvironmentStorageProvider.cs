using AutomationIoC.Runtime.Session;
using System.Management.Automation;

namespace AutomationIoC.Runtime.Environment
{
    internal class EnvironmentStorageProvider : IEnvironmentStorageProvider
    {
        private readonly ISessionState sessionState;

        public EnvironmentStorageProvider(ISessionState sessionState)
        {
            this.sessionState = sessionState;
        }

        public T GetEnvironmentVariable<T>(string key)
        {
            PSVariable psVariable = sessionState.PSVariable.Get(key);

            if (psVariable?.Value is not null)
            {
                return (T)psVariable.Value;
            }

            return default;
        }

        public void SetEnvironmentVariable(string key, object value, ScopedItemOptions scopedItemOptions)
        {
            PSVariable serviceVariable =
                    new(key, value, scopedItemOptions);

            sessionState.PSVariable.Set(serviceVariable);
        }
    }
}
