using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutomationIoC
{

    public abstract class AutomationStartup : IoCShellBase
    {
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

            Context.GenerateServices(serviceCollection);

            WriteVerbose("Application Ready");
        }
    }
}
