using AutomationIoC.Integration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Commands
{
    public class TestStartup : IoCStartup
    {
        public override void Configure(IConfigurationBuilder configurationBuilder)
        {
            var appSettings = new Dictionary<string, string>()
            {
                ["testOptions:mode"] = "basic-test",
            };

            configurationBuilder.AddInMemoryCollection(appSettings);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITestService, TestService>();
        }
    }
}
