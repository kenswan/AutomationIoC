using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Management.Automation;

namespace PowerShellFocused
{

    public abstract class FocusedStartup : FocusedCmdletBase
    {
        public const string SERVICE_PROVIDER = "ServiceProvider";
        protected IConfiguration Configuration { get; set; }
        protected IServiceProvider ServiceProvider { get; private set; }

        private readonly IConfigurationBuilder configurationBuilder;
        private readonly IServiceCollection serviceCollection;

        public FocusedStartup()
        {
            configurationBuilder = new ConfigurationBuilder();
            serviceCollection = new ServiceCollection();
        }

        public abstract void ConfigureServices(IServiceCollection services);

        public abstract void Configure(IConfigurationBuilder configurationBuilder);

        protected override void ProcessRecord()
        {
            WriteVerbose("Starting Dependency Injection Creation");

            Configure(configurationBuilder);

            Configuration = configurationBuilder.Build();

            WriteVerbose("Configured Configuration");

            serviceCollection.AddSingleton(Configuration);

            serviceCollection.AddLogging(builder => builder.AddConsole());

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            WriteVerbose("Configured Services");

            if (SessionState is not null)
            {
                PSVariable serviceVariable = new(SERVICE_PROVIDER, ServiceProvider, ScopedItemOptions.ReadOnly);

                SessionState.PSVariable.Set(serviceVariable);
            }
            else
            {
                using var scope = ServiceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<FocusedStartup>>();

                logger.LogWarning(
                    @"PowerShell Environment has not been detected. 
                        If this is not a test, please connect to PowerShell");
            }

            WriteVerbose("Application Ready");
        }
    }
}
