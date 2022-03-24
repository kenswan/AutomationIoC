using AutomationIoC.Integration.Services;
using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Commands
{
    public class TestStartup : IIoCStartup
    {
        public IConfiguration Configuration { get; set; }

        public void Configure(IConfigurationBuilder configurationBuilder)
        {
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITestService, TestService>();
        }
    }
}
