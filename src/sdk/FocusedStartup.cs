using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace PowerShellFocused
{
    public abstract class FocusedStartup : PSCmdlet
    {
        public const string SERVICE_PROVIDER = "ServiceProvider";
        protected IConfiguration Configuration { get; set; }

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

            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            WriteVerbose("Configured Services");

            PSVariable serviceVariable = new(SERVICE_PROVIDER, serviceProvider, ScopedItemOptions.ReadOnly);

            SessionState.PSVariable.Set(serviceVariable);

            WriteVerbose("Application Ready");
        }
    }
}
