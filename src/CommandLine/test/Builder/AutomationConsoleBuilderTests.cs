// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Builder;
using AutomationIoC.CommandLine.Test.TestBed.Commands;
using AutomationIoC.CommandLine.Test.TestBed.Services;
using AutomationIoC.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.Builder;

public class AutomationConsoleBuilderTests
{
    [Fact]
    public void ShouldBuildCommandsWithAddCommand()
    {
        // Arrange
        var automationContext = new AutomationContext();
        var rootCommand = new AutomationCommand(RootCommand.ExecutableName, "Test Application", automationContext);
        var automationConsoleBuilder = new AutomationConsoleBuilder(rootCommand, automationContext);

        automationConsoleBuilder.AddCommand<BasicTestCommandInitializer>("test", "subcommand");

        AutomationCommand configuredRootCommand = automationConsoleBuilder.GetRootCommand();

        // Act
        int invocationResult =
            configuredRootCommand.Parse(["test", "subcommand", "--optionOne", "testValue"])
                .Invoke();

        // Assert
        Assert.Equal(0, invocationResult);
    }

    [Fact]
    public void ShouldBuildCommandsWithAutomationContext()
    {
        // Arrange
        const string expectedKey = "TestTheConfigKey";
        string expectedKeyValue = Guid.NewGuid().ToString();
        var automationContext = new AutomationContext();
        var rootCommand = new AutomationCommand(RootCommand.ExecutableName, "Test Application", automationContext);
        var automationConsoleBuilder = new AutomationConsoleBuilder(rootCommand, automationContext);
        var testServiceMock = new Mock<ITestService>();

        automationConsoleBuilder.AddCommand<FullTestCommandInitializer>("full-test");

        automationConsoleBuilder.Configure((context, configurationBuilder) =>
        {
            var appSettings = new Dictionary<string, string>
            {
                {
                    expectedKey, expectedKeyValue
                }
            };
            configurationBuilder.AddInMemoryCollection(appSettings);
        });

        automationConsoleBuilder.ConfigureServices((context, services) =>
        {
            services.AddTransient(provider => testServiceMock.Object);
            services.AddTransient<TestConfigurationService>();
        });

        AutomationCommand configuredRootCommand = automationConsoleBuilder.GetRootCommand();

        // Act
        int invocationResult =
            configuredRootCommand.Parse(["full-test", "--key", expectedKey])
                .Invoke();

        // Assert
        Assert.Equal(0, invocationResult);
        testServiceMock.Verify(service => service.Execute(expectedKeyValue), Times.Once);
    }

    [Fact]
    public void ShouldBuildAutomationConsoleApplication()
    {
        // Arrange
        const string expectedKey = "TestTheConfigKey";
        string expectedKeyValue = Guid.NewGuid().ToString();
        var automationContext = new AutomationContext();

        var rootCommand =
            new AutomationCommand(RootCommand.ExecutableName, "Test Application", automationContext);

        var automationConsoleBuilder =
            new AutomationConsoleBuilder(rootCommand, automationContext, ["full-test", "--key", expectedKey]);

        var testServiceMock = new Mock<ITestService>();

        automationConsoleBuilder.AddCommand<FullTestCommandInitializer>("full-test");

        automationConsoleBuilder.Configure((context, configurationBuilder) =>
        {
            var appSettings = new Dictionary<string, string>
            {
                {
                    expectedKey, expectedKeyValue
                }
            };
            configurationBuilder.AddInMemoryCollection(appSettings);
        });

        automationConsoleBuilder.ConfigureServices((context, services) =>
        {
            services.AddTransient(provider => testServiceMock.Object);
            services.AddTransient<TestConfigurationService>();
        });

        // Act
        int invocationResult =
            automationConsoleBuilder.Build().Run();

        // Assert
        Assert.Equal(0, invocationResult);
        testServiceMock.Verify(service => service.Execute(expectedKeyValue), Times.Once);
    }
}
