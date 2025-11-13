// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Services;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.TestBed.Commands;

public class FullTestCommandInitializer : IAutomationCommandInitializer
{
    public void Initialize(IAutomationCommand command)
    {
        Option<string> configurationKeyOption = new(name: "--key")
        {
            Description = "Name of key to pull from configuration."
        };

        command.Add(configurationKeyOption);

        command.SetAction(async (parseResult, context, cancellationToken) =>
        {
            string configurationKey = parseResult.GetValue(configurationKeyOption);

            IServiceProvider serviceProvider = context.ServiceProvider;

            TestConfigurationService testConfigurationService =
                serviceProvider.GetRequiredService<TestConfigurationService>();

            ITestService testService =
                serviceProvider.GetRequiredService<ITestService>();

            string configurationValue =
                await testConfigurationService
                    .GetConfigurationValueAsync(configurationKey, cancellationToken)
                    .ConfigureAwait(false);

            testService.Execute(configurationValue);
        });
    }
}
