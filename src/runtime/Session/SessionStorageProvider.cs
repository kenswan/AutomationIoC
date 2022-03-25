using System.Management.Automation;

namespace AutomationIoC.Runtime.Session
{
    internal class SessionStorageProvider : ISessionStorageProvider
    {
        private readonly ISessionState sessionState;
        private readonly IIoCStartup startup;

        private string StorageKey => startup.GetType().Name;

        public SessionStorageProvider(ISessionState sessionState, IIoCStartup startup)
        {
            this.sessionState = sessionState;
            this.startup = startup;
        }

        public IServiceProvider GetCurrentServiceProvider()
        {
            PSVariable psVariable = sessionState.PSVariable.Get(StorageKey);

            if (psVariable?.Value is not null)
            {
                return (IServiceProvider)psVariable.Value;
            }

            return null;
        }

        public void StoreServiceProvider(IServiceProvider serviceProvider)
        {
            PSVariable serviceVariable =
                    new(StorageKey, serviceProvider, ScopedItemOptions.ReadOnly);

            sessionState.PSVariable.Set(serviceVariable);
        }
    }
}
