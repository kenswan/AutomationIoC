using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Management.Automation;

namespace AutomationIoC
{

    public abstract class AutomationStartup : IoCShellBase
    {
        public const string SERVICE_PROVIDER = "ServiceProvider";
        
        protected IConfiguration Configuration { get; set; }

        public abstract void ConfigureServices(IServiceCollection services);

        public abstract void Configure(IConfigurationBuilder configurationBuilder);

        protected sealed override void ProcessRecord()
        {
            WriteVerbose("Starting Dependency Injection Creation");

            var configurationBuilder = new ConfigurationBuilder();
            
            Configure(configurationBuilder);

            Configuration = configurationBuilder.Build();

            WriteVerbose("Configured Configuration");

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(Configuration);

            serviceCollection.AddLogging(builder => builder.AddConsole());

            ConfigureServices(serviceCollection);

            WriteVerbose("Configured Services");

            if (SessionState is not null)
            {
                PSVariable serviceVariable =
                    new(SERVICE_PROVIDER, serviceCollection.BuildServiceProvider(), ScopedItemOptions.ReadOnly);

                SessionState.PSVariable.Set(serviceVariable);
            }
            else
            {
                using var serviceProvider = serviceCollection.BuildServiceProvider();
                var logger = serviceProvider.GetRequiredService<ILogger<AutomationStartup>>();

                logger.LogWarning(
                    @"PowerShell Environment has not been detected. 
                        If this is not a test, please connect to PowerShell");
            }

            WriteVerbose("Application Ready");
        }
    }
}
