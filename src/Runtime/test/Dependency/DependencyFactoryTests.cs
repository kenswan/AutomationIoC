// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorFocused.Automation.Runtime.Dependency;
using BlazorFocused.Automation.Runtime.Test.TestBed.Models;
using BlazorFocused.Automation.Runtime.Test.TestBed.Services;
using BlazorFocused.Automation.Runtime.Test.TestBed.Startup;

namespace BlazorFocused.Automation.Runtime.Test.Dependency;

public class DependencyFactoryTests
{
    private const string DEFAULT_ENVIRONMENT_NAME = "Production";

    [Fact]
    public void GenerateServiceProvider_ShouldBuildRuntimeProviderFromSession()
    {
        // Arrange
        var sessionStorage = new TestSessionStorage();
        var startup = new TestRuntimeStartup();

        // Act
        IServiceProvider actualServiceProvider = DependencyFactory.GenerateServiceProvider(sessionStorage, startup);

        // Assert
        actualServiceProvider.GetRequiredService<ISessionStorage>()
            .Should().BeEquivalentTo(sessionStorage);

        actualServiceProvider.GetRequiredService<IAutomationStartup>()
            .Should().BeEquivalentTo(startup);

        Assert.NotNull(actualServiceProvider.GetService<IAutomationBinder>());
        Assert.NotNull(actualServiceProvider.GetService<IContextBuilder>());
    }

    [Fact]
    public void GenerateServiceProvider_ShouldBuildRuntimeProviderFromSessionStatProxy()
    {
        // Arrange
        var sessionStorage = new TestSessionStorage();

        // Act
        IServiceProvider actualServiceProvider = DependencyFactory.GenerateServiceProvider(sessionStorage);

        // Assert
        actualServiceProvider.GetRequiredService<ISessionStorage>()
            .Should().BeEquivalentTo(sessionStorage);
    }

    [Fact]
    public void GenerateHost_ShouldGenerateAutomationHost()
    {
        // Arrange
        var testStartup = new TestRuntimeStartup();

        // Act
        using IHost host = DependencyFactory.GenerateHost(
            testStartup.Configure,
            testStartup.ConfigureServices);

        IServiceProvider serviceProvider = host.Services;
        IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
        IHostEnvironment hostEnvironment = serviceProvider.GetService<IHostEnvironment>();

        // Assert
        Assert.NotNull(serviceProvider.GetRequiredService<ITestRuntimeService>());
        Assert.NotNull(serviceProvider.GetRequiredService<ITestRuntimeInternalServiceOne>());
        Assert.NotNull(serviceProvider.GetRequiredService<ITestRuntimeInternalServiceTwo>());

        Assert.Equal(DEFAULT_ENVIRONMENT_NAME, hostEnvironment.EnvironmentName);

        Assert.Equal(
            TestRuntimeStartup.CONFIGURATION_VALUE,
            configuration.GetValue<string>(TestRuntimeStartup.CONFIGURATION_KEY));
    }

    [Fact]
    public void GenerateHost_ShouldBeOverriddenByEnvironmentParameters()
    {
        // Arrange
        var testStartup = new TestRuntimeStartup();
        string expectedEnvironmentName = "TestEnvironment";
        string expectedConfigurationKey = "TestEnvironmentConfigKey";
        string expectedConfigurationValue = "TestEnvironmentConfigValue";

        string[] inputParameters =
            new string[]
            {
                "--environment", expectedEnvironmentName,
                "--testParameter", expectedConfigurationValue
            };

        IDictionary<string, string> inputConfigurationParameterMapping =
            new Dictionary<string, string>()
            {
                { "--testParameter", expectedConfigurationKey }
            };

        // Act
        using IHost host = DependencyFactory.GenerateHost(
            testStartup.Configure,
            testStartup.ConfigureServices,
            inputParameters,
            inputConfigurationParameterMapping);

        IServiceProvider serviceProvider = host.Services;
        IHostEnvironment hostEnvironment = serviceProvider.GetService<IHostEnvironment>();
        IConfiguration configuration = serviceProvider.GetService<IConfiguration>();

        string actualEnvironmentName = hostEnvironment.EnvironmentName;
        string actualConfigurationValue = configuration.GetValue<string>(expectedConfigurationKey);

        // Assert
        Assert.Equal(expectedEnvironmentName, hostEnvironment.EnvironmentName);
        Assert.Equal(expectedConfigurationValue, actualConfigurationValue);
    }
}
