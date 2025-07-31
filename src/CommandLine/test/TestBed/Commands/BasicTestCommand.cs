// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.TestBed.Commands;

internal class BasicTestCommand : IAutomationCommand
{
    private const string InternalTestData = "test";

    public void Initialize(AutomationCommand command)
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

        command.Options.Add(passedInOption);
        command.Options.Add(internalOption);

        command.SetAction(parseResult =>
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

internal class BasicTestCommand2 : BasicTestCommand
{
}

internal class BasicTestCommand3 : BasicTestCommand
{
}
