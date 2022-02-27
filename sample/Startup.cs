using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutomationIoC.Sample.Models;
using System.Management.Automation;

namespace AutomationIoC.Sample
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
            services.AddSingleton<Deck>();
        }
    }
}
