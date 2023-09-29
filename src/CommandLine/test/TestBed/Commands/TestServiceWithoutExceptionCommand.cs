// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using BlazorFocused.Automation.CommandLine.Test.TestBed.Services;

namespace BlazorFocused.Automation.CommandLine.Test.TestBed.Commands;

internal class TestServiceWithoutExceptionCommand : StandardCommand
{
    public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
    {
        var passedInOption = new Option<string>(
            name: "--test",
            description: "Description of test.");

        command.AddOption(passedInOption);

        command.SetHandler(TestExecution,
            serviceBinderFactory.Bind<ITestService>(),
            passedInOption);
    }

    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            [TestService.CONFIG_KEY] = TestService.CONFIG_VALUE,
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) =>
        services.AddTransient<ITestService, TestService>();

    private static void TestExecution(ITestService testService, string data) => testService.Execute(data);
}
