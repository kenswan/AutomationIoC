// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationIoC.Runtime.Startup;

public class TestHostBuildContextStartup : IIoCStartup
{
    private readonly string environmentName;
    private readonly string connectionString;
    private readonly string configurationCheckKey;
    private readonly string configurationCheckValue;
    private readonly string connectionStringKey = "TestDefaultConnection";

    public TestHostBuildContextStartup(
        string environmentName,
        string connectionString,
        string configurationCheckKey,
        string configurationCheckValue)
    {
        this.environmentName = environmentName;
        this.connectionString = connectionString;
        this.configurationCheckKey = configurationCheckKey;
        this.configurationCheckValue = configurationCheckValue;
    }

    public IConfiguration Configuration { get; set; }

    public IAutomationEnvironment AutomationEnvironment { get; set; }

    public void Configure(IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            [configurationCheckKey] = configurationCheckValue,
            ["DOTNET_ENVIRONMENT"] = environmentName,
            [$"ConnectionStrings:{connectionStringKey}"] = connectionString
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
        configurationBuilder.AddEnvironmentVariables();
    }

    public void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        string environmentName = hostBuilderContext.HostingEnvironment.EnvironmentName;
        string connectionString = hostBuilderContext.Configuration.GetConnectionString(connectionStringKey);
        string configurationValue = hostBuilderContext.Configuration.GetValue<string>(configurationCheckKey);

        services.AddScoped(_ => new TestHostBuilderContextService(
            environmentName: environmentName,
            connectionString: connectionString,
            configurationValue: configurationValue));
    }
}
