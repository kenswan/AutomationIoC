// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Services;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.TestBed.Commands;

internal class TestServiceWithoutExceptionCommand : IAutomationCommand
{
    public void Initialize(AutomationCommand command)
    {
        Option<string> passedInOption = new(name: "--test")
        {
            Description = "Description of test."
        };

        command.Options.Add(passedInOption);

        command.SetAction((parseResult, automationContext) =>
        {
            string passedInOptionString = parseResult.GetValue(passedInOption);
            TestExecution(automationContext.ServiceProvider.GetService<ITestService>(), passedInOptionString);
        });
    }

    // public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    // {
    //     var appSettings = new Dictionary<string, string>
    //     {
    //         [TestService.CONFIG_KEY] = TestService.CONFIG_VALUE
    //     };
    //
    //     configurationBuilder.AddInMemoryCollection(appSettings);
    // }
    //
    // public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) =>
    //     services.AddTransient<ITestService, TestService>();

    private static void TestExecution(ITestService testService, string data) => testService.Execute(data);
}
