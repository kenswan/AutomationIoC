using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace AutomationIoC.Runtime
{
    public interface IIoCStartup
    {
        IConfiguration Configuration { get; internal set; }

        void Configure(IConfigurationBuilder configurationBuilder);

        void ConfigureServices(IServiceCollection services);
    }
}
