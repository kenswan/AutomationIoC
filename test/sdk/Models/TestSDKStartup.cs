using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Models
{
    public class TestSDKStartup : IIoCStartup
    {
        public IConfiguration Configuration { get; set; }

        public IAutomationEnvironment AutomationEnvironment { get; set; }

        public void Configure(IConfigurationBuilder configurationBuilder)
        {
            throw new NotImplementedException();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }
}
