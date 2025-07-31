﻿// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Services;
using AutomationIoC.Runtime.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test;

public class AutomationCommandTests
{
    [Fact]
    public void CreateCommand_ShouldInitializeWithNameAndDescription()
    {
        // Arrange
        const string commandName = "testCommand";
        const string commandDescription = "This is a test command";
        var automationContext = new AutomationContext();

        // Act
        var command = new AutomationCommand(
            name: commandName,
            description: commandDescription,
            automationContext: automationContext);

        // Assert
        Assert.Equal(commandName, command.Name);
        Assert.Equal(commandDescription, command.Description);
    }

    [Fact]
    public void CreateCommand_ShouldCreateCommandWithAutomationContext()
    {
        // Arrange
        const string commandName = "testCommand";
        const string configurationKey = "keyOne";
        const string configurationValue = "valueOne";
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
            services.AddTransient<TestConfigurationService>();
        });

        var command = new AutomationCommand(
            name: commandName,
            description: "Test Command Description",
            automationContext);

        string actualConfigurationValue = string.Empty;

        // Act
        command.SetAction((parseResult, context) =>
        {
            IServiceProvider serviceProvider = context.ServiceProvider;
            TestConfigurationService testService = serviceProvider.GetRequiredService<TestConfigurationService>();
            actualConfigurationValue = testService.GetConfigurationValue(configurationKey);
        });

        new CommandLineConfiguration(command).Invoke(commandName);

        // Assert
        Assert.Equal(configurationValue, actualConfigurationValue);
    }

    [Fact]
    public async Task CreateCommand_ShouldCreateCommandWithAutomationContextAsyncAction()
    {
        // Arrange
        const string commandName = "testCommand";
        const string configurationKey = "keyOne";
        const string configurationValue = "valueOne";
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
            services.AddTransient<TestConfigurationService>();
        });

        var command = new AutomationCommand(
            name: commandName,
            description: "Test Command Description",
            automationContext);

        string actualConfigurationValue = string.Empty;

        // Act
        command.SetAction(async (parseResult, context, cancellationToken) =>
        {
            IServiceProvider serviceProvider = context.ServiceProvider;

            TestConfigurationService testService =
                serviceProvider.GetRequiredService<TestConfigurationService>();

            actualConfigurationValue =
                await testService
                    .GetConfigurationValueAsync(configurationKey, cancellationToken)
                    .ConfigureAwait(false);
        });

        await new CommandLineConfiguration(command)
            .InvokeAsync(commandName, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(configurationValue, actualConfigurationValue);
    }

    [Fact]
    public async Task Clone_ShouldCloneCommands()
    {
        // Arrange
        const string commandOneName = "testCommandOne";
        const string commandTwoName = "testCommandTwo";
        const string commandOptionValue = "test-option-value";
        const string configurationKey = "keyOne";
        const string configurationValue = "valueOne";
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
            services.AddTransient<TestConfigurationService>();
        });

        var commandOne = new AutomationCommand(
            name: commandOneName,
            description: "Test Command Description",
            automationContext);

        var commandOneSubCommand = new AutomationCommand(
            name: commandOneName,
            description: "Test Command Description",
            automationContext);

        commandOne.Subcommands.Add(commandOneSubCommand);

        var commandTwo = new Command(
            name: commandTwoName,
            description: "Test Command Description");

        string actualConfigurationValue = string.Empty;
        string actualOptionValue = string.Empty;

        Option<string> passedInOption = new(name: "--optionOne")
        {
            Description = "Description of option one field."
        };

        commandOne.Options.Add(passedInOption);

        commandOne.SetAction(async (parseResult, context, cancellationToken) =>
        {
            actualOptionValue = parseResult.GetValue(passedInOption);

            IServiceProvider serviceProvider = context.ServiceProvider;

            TestConfigurationService testService =
                serviceProvider.GetRequiredService<TestConfigurationService>();

            actualConfigurationValue =
                await testService
                    .GetConfigurationValueAsync(configurationKey, cancellationToken)
                    .ConfigureAwait(false);
        });

        // Act
        AutomationCommand.Clone(source: commandOne, target: commandTwo);

        await new CommandLineConfiguration(commandTwo)
            .InvokeAsync([commandTwoName, "--optionOne", commandOptionValue], TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(configurationValue, actualConfigurationValue);
        Assert.Equal(commandOptionValue, actualOptionValue);
        Assert.Equivalent(commandOne.Options, commandTwo.Options);
        Assert.Equivalent(commandOne.Subcommands, commandTwo.Subcommands);
        Assert.Equivalent(commandOne.Arguments, commandTwo.Arguments);
        Assert.Equivalent(commandOne.Aliases, commandTwo.Aliases);
    }
}
