// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Test.TestBed.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Test.Context;

public class AutomationServiceActivatorTests
{
    [Fact]
    public void GetServiceProvider_ShouldConfigureServices()
    {
        // Arrange
        const string configurationKey = "TestConfiguration";
        const string configurationValue = "ExecutedConfiguration";
        const string expectedFieldOneValue = "ExecutedConfiguration";
        const string expectedFieldTwoValue = "ExecutedConfiguration2";

        var activator = new AutomationServiceActivator();

        activator.SetConfiguration((context, configurationBuilder) =>
        {
            var appSettings = new Dictionary<string, string>
            {
                {
                    configurationKey, configurationValue
                }
            };
            configurationBuilder.AddInMemoryCollection(appSettings);
        });

        activator.SetServices((context, services) =>
        {
            services.Configure<TestConfiguredHostServiceOptions>(configureOptions =>
            {
                configureOptions.FieldOne = expectedFieldOneValue;
                configureOptions.FieldTwo = expectedFieldTwoValue;
            });

            services.AddScoped<TestConfiguredHostService>();
        });

        // Act
        IServiceProvider? serviceProvider = activator.GetServiceProvider();

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
    public void GetServiceProvider_ShouldReadConfigurationValuesFromArguments()
    {
        // Arrange
        const string fieldOneKey = "TestConfiguredHostServiceOptions:FieldOne";
        const string expectedFieldOneValue = "TestConfigurationValueOne";
        const string fieldTwoKey = "TestConfiguredHostServiceOptions:FieldTwo";
        const string expectedFieldTwoValue = "TestConfigurationValueTwo";

        var activator = new AutomationServiceActivator();

        activator.SetServices((context, services) =>
        {
            services.AddScoped<TestConfiguredHostService>();

            services.AddOptions<TestConfiguredHostServiceOptions>()
                .BindConfiguration(nameof(TestConfiguredHostServiceOptions));
        });

        activator.SetConfigurationMapping(new Dictionary<string, string>
        {
            ["--fieldOne"] = fieldOneKey,
            ["--fieldTwo"] = fieldTwoKey
        });

        activator.SetArgs(["--fieldOne", expectedFieldOneValue, "--fieldTwo", expectedFieldTwoValue]);

        // Act
        IServiceProvider? serviceProvider = activator.GetServiceProvider();

        // Assert
        Assert.NotNull(serviceProvider);

        TestConfiguredHostService? testService =
            serviceProvider.GetService<TestConfiguredHostService>();

        Assert.NotNull(testService);
        Assert.True(testService.GetKeyValue(fieldOneKey, out string? actualFieldOneValue));
        Assert.Equal(expectedFieldOneValue, actualFieldOneValue);
        Assert.True(testService.GetKeyValue(fieldTwoKey, out string? actualFieldTwoValue));
        Assert.Equal(expectedFieldTwoValue, actualFieldTwoValue);
    }
}
