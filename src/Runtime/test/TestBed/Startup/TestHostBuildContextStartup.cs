// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutomationIoC.Runtime.Test.TestBed.Services;

namespace AutomationIoC.Runtime.Test.TestBed.Startup;

public class TestHostBuildContextStartup : AutomationStartup
{
    public const string ENVIRONMENT_NAME = $"{nameof(TestHostBuildContextStartup)}Env";

    private readonly string connectionString;
    private readonly string configurationCheckKey;
    private readonly string configurationCheckValue;
    private readonly string connectionStringKey = "TestDefaultConnection";

    public TestHostBuildContextStartup(
        string connectionString,
        string configurationCheckKey,
        string configurationCheckValue)
    {
        this.connectionString = connectionString;
        this.configurationCheckKey = configurationCheckKey;
        this.configurationCheckValue = configurationCheckValue;
    }

    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            [configurationCheckKey] = configurationCheckValue,
            [$"ConnectionStrings:{connectionStringKey}"] = connectionString
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
        configurationBuilder.AddEnvironmentVariables();
    }

    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        string environmentName = hostBuilderContext.HostingEnvironment.EnvironmentName;
        string connectionString = hostBuilderContext.Configuration.GetConnectionString(connectionStringKey);
        string configurationValue = hostBuilderContext.Configuration.GetValue<string>(configurationCheckKey);

        services.AddScoped(_ => new TestHostBuilderContextService(
            environmentName: environmentName,
            connectionString: connectionString,
            configurationValue: configurationValue));

        services
            .AddTransient<ITestRuntimeService, TestRuntimePropertyService>()
            .AddTransient<ITestRuntimeService, TestRuntimeFieldService>()
            .AddTransient<ITestRuntimeInternalServiceOne, TestRuntimeInternalServiceOne>()
            .AddTransient<ITestRuntimeInternalServiceTwo, TestRuntimeInternalServiceTwo>();
    }

    public override string[] GenerateParameters() =>
        new string[]
        {
            "--environment", ENVIRONMENT_NAME,
            "--testParam", "ThisIsATest"
        };

    public override IDictionary<string, string> GenerateParameterConfigurationMapping() =>
        new Dictionary<string, string>()
        {
            { "--testParam", "ConfiguredTestParameter" }
        };
}
