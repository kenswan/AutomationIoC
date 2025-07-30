// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Test.TestBed.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Test.Context;

public class AutomationContextTests
{
    [Fact]
    public void ServiceProvider_ShouldConfigureAutomationServiceActivatorServicesWithinContext()
    {
        // Arrange
        const string configurationKey = "TestConfiguration";
        const string configurationValue = "ExecutedConfiguration";
        const string expectedFieldOneValue = "ExecutedConfiguration";
        const string expectedFieldTwoValue = "ExecutedConfiguration2";

        var automationContext = new AutomationContext();

        automationContext.SetConfigure((context, configurationBuilder) =>
        {
            var appSettings = new Dictionary<string, string>
            {
                {
                    configurationKey, configurationValue
                }
            };
            configurationBuilder.AddInMemoryCollection(appSettings);
        });

        automationContext.SetConfigureServices((context, services) =>
        {
            services.Configure<TestConfiguredHostServiceOptions>(configureOptions =>
            {
                configureOptions.FieldOne = expectedFieldOneValue;
                configureOptions.FieldTwo = expectedFieldTwoValue;
            });

            services.AddScoped<TestConfiguredHostService>();
        });

        // Act
        IServiceProvider? serviceProvider = automationContext.ServiceProvider;

        // Assert
        Assert.NotNull(serviceProvider);

        TestConfiguredHostService? testService =
            serviceProvider.GetService<TestConfiguredHostService>();

        Assert.NotNull(testService);
        Assert.True(testService.GetKeyValue(configurationKey, out string? value));
        Assert.Equal(configurationValue, value);
        Assert.Equal(expectedFieldOneValue, testService.FieldOne);
        Assert.Equal(expectedFieldTwoValue, testService.FieldTwo);
    }

    [Fact]
    public void Configuration_ShouldSetConfigurationValuesFromArgumentsWithinContext()
    {
        // Arrange
        const string configurationKey = "ConfigurationKey";
        const string expectedConfigurationValue = "ConfigurationKeyValue";

        var automationContext = new AutomationContext();

        automationContext.SetConfigurationMapping(new Dictionary<string, string>
        {
            ["--testConfigs"] = configurationKey
        });

        automationContext.SetArgs(["--testConfigs", expectedConfigurationValue]);

        // Act
        IConfiguration? configuration = automationContext.Configuration;

        // Assert
        Assert.NotNull(configuration);
        Assert.Equal(expectedConfigurationValue, configuration.GetValue<string>(configurationKey));
    }
}
