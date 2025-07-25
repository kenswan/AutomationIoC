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

internal class TestServiceWithExceptionCommand : StandardCommand
{
    public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
    {
        Option<string> passedInOption = new(name: "--test")
        {
            Description = "Description of test option field."
        };

        command.SetAction(parseResult =>
        {
            string passedInOptionString = parseResult.GetValue(passedInOption);
            TestExecution(serviceBinderFactory.Bind<ITestService>(), passedInOptionString);
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

    private static void TestExecution(ITestService testService, string data) => testService.Execute(data);
}
