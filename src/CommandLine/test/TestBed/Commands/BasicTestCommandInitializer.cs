// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.TestBed.Commands;

internal class BasicTestCommandInitializer : IAutomationCommandInitializer
{
    private const string InternalTestData = "test";

    public void Initialize(IAutomationCommand command)
    {
        Option<string> passedInOption = new(name: "--optionOne")
        {
            Description = "Description of option one field."
        };

        Option<string> internalOption = new(name: "--optionTwo")
        {
            Description = "Description of option two field.",
            DefaultValueFactory = _ => InternalTestData
        };

        command.Add(passedInOption);
        command.Add(internalOption);

        command.SetAction((parseResult, automationContext) =>
        {
            string passedInOptionString = parseResult.GetValue(passedInOption);
            string internalOptionString = parseResult.GetValue(internalOption);
            Console.WriteLine($"Passed in option: {passedInOptionString}");
            Console.WriteLine($"Internal option: {internalOptionString}");
            TestData(passedInOptionString);
        });
    }

    private static void TestData(string data)
    {
        Console.WriteLine("options 1:" + data);
        Console.WriteLine("option 2:" + InternalTestData);
    }
}

internal class BasicTestCommand2 : BasicTestCommandInitializer
{
}

internal class BasicTestCommand3 : BasicTestCommandInitializer
{
}
