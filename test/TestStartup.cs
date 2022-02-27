using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace PowerShellFocused.Test
{
    [Cmdlet(VerbsLifecycle.Build, "Dependencies")]
    public class TestStartup : FocusedStartup
    {
        public void RunInstance()
        {
            BeginProcessing();
            ProcessRecord();
            EndProcessing();
        }

        public override void Configure(IConfigurationBuilder configurationBuilder)
        {
            // throw new NotImplementedException();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TestService>();
        }
    }
}
