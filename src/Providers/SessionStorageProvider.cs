using System.Management.Automation;

namespace AutomationIoC.Providers
{
    internal class SessionStorageProvider : ISessionStorageProvider
    {
        private const string SESSION_SERVICE_PROVIDER = "PSServiceProvider";

        private readonly SessionState sessionState;

        public SessionStorageProvider(SessionState sessionState)
        {
            this.sessionState = sessionState;
        }

        public IServiceProvider GetServiceProvider()
        {
            PSVariable psVariable = sessionState.PSVariable.Get(SESSION_SERVICE_PROVIDER);

            if (psVariable?.Value is not null)
            {
                return (IServiceProvider)psVariable.Value;
            }
            else
            {
                throw new ArgumentNullException(
                    nameof(ISessionStorageProvider),
                    "Session provider is missing. Ensure Service Focused Startup was called.");
            }
        }

        public void StoreProvider(IServiceProvider serviceProvider)
        {
            PSVariable serviceVariable =
                    new(SESSION_SERVICE_PROVIDER, serviceProvider, ScopedItemOptions.ReadOnly);

            sessionState.PSVariable.Set(serviceVariable);
        }
    }
}
