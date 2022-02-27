using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PowerShellFocused.Sample.Models;
using System.Management.Automation;

namespace PowerShellFocused.Sample
{
    [Cmdlet(VerbsLifecycle.Start, "Game")]
    public class Startup : FocusedStartup
    {
        public override void Configure(IConfigurationBuilder configurationBuilder)
        {
            var appSettings = new Dictionary<string, string>()
            {
                ["player:mode"] = "Beginner",
            };

            configurationBuilder.AddInMemoryCollection(appSettings);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddConsole());
            services.AddSingleton<Deck>();
        }
    }
}
