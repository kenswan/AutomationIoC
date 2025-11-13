// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Services;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.TestBed.Commands;

internal class TestServiceWithoutExceptionCommandInitializer : IAutomationCommandInitializer
{
    public void Initialize(IAutomationCommand command)
    {
        Option<string> passedInOption = new(name: "--test")
        {
            Description = "Description of test."
        };

        command.Add(passedInOption);

        command.SetAction((parseResult, automationContext) =>
        {
            string passedInOptionString = parseResult.GetValue(passedInOption);
            TestExecution(automationContext.ServiceProvider.GetService<ITestService>(), passedInOptionString);
        });
    }

    private static void TestExecution(ITestService testService, string data) => testService.Execute(data);
}
