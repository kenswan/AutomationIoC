using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime
{
    public interface IIoCStartup
    {
        IConfiguration Configuration { get; internal set; }

        void ConfigureServices(IServiceCollection services);

        void Configure(IConfigurationBuilder configurationBuilder);
    }
}
