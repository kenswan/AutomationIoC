// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Services;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.TestBed.Commands;

internal class TestServiceWithAsyncCommandInitializer : IAutomationCommandInitializer
{
    public void Initialize(IAutomationCommand command)
    {
        Option<string> passedInOption = new(name: "--test")
        {
            Description = "Description of test option field."
        };

        command.Add(passedInOption);

        command.SetAction(async (parseResult, automationContext, cancellationToken) =>
        {
            string passedInOptionString = parseResult.GetValue(passedInOption);
            await TestExecutionAsync(
                    automationContext.ServiceProvider.GetService<ITestService>(),
                    passedInOptionString,
                    cancellationToken)
                .ConfigureAwait(false);
        });
    }

    // public static void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    // {
    //     var appSettings = new Dictionary<string, string>
    //     {
    //         [TestService.CONFIG_KEY] = TestService.CONFIG_VALUE
    //     };
    //
    //     configurationBuilder.AddInMemoryCollection(appSettings);
    // }
    //
    // public static void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) =>
    //     services.AddTransient<ITestService, TestService>();

    private static Task TestExecutionAsync(ITestService testService, string data, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Testing token can be cancelled: {cancellationToken.CanBeCanceled}");
        testService.Execute(data);

        return Task.CompletedTask;
    }
}
