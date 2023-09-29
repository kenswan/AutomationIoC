// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace BlazorFocused.Automation.CommandLine.Test.TestBed.Commands;

internal class BasicTestCommand : StandardCommand
{
    private readonly string internalTestData;

    public BasicTestCommand()
    {
        internalTestData = "test";
    }

    public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
    {
        var passedInOption = new Option<string>(
            name: "--optionOne",
            description: "Description of option field.");

        var internalOption = new Option<string>(
            name: "--optionTwo",
            description: "Description of option field.", getDefaultValue: () => internalTestData);

        command.AddOption(passedInOption);
        command.AddOption(internalOption);

        command.SetHandler(TestData, passedInOption);
    }

    private void TestData(string data)
    {
        Console.WriteLine(internalTestData);
        Console.WriteLine(data);
    }
}

internal class BasicTestCommand2 : BasicTestCommand { }

internal class BasicTestCommand3 : BasicTestCommand { }

