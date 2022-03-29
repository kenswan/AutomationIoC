using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime
{
    public interface IIoCStartup
    {
        IAutomationEnvironment AutomationEnvironment { get; set; }

        IConfiguration Configuration { get; set; }

        void Configure(IConfigurationBuilder configurationBuilder);

        void ConfigureServices(IServiceCollection services);
    }
}
