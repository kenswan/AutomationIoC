// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.TestBed.Commands;

internal class TestServiceWithAsyncCommand : StandardCommand
{
    public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
    {
        Option<string> passedInOption = new(name: "--test")
        {
            Description = "Description of test option field."
        };

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            string passedInOptionString = parseResult.GetValue(passedInOption);
            await TestExecutionAsync(
                    serviceBinderFactory.Bind<ITestService>(),
                    passedInOptionString,
                    cancellationToken)
                .ConfigureAwait(false);
        });
    }

    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>
        {
            [TestService.CONFIG_KEY] = TestService.CONFIG_VALUE
        };

        configurationBuilder.AddInMemoryCollection(appSettings);

        throw new NotImplementedException();
    }

    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        services.AddTransient<ITestService, TestService>();

        throw new NotImplementedException();
    }

    private static Task TestExecutionAsync(ITestService testService, string data, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Testing token can be cancelled: {cancellationToken.CanBeCanceled}");
        testService.Execute(data);

        return Task.CompletedTask;
    }
}
