// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.Consoles;

public class AutomationIoConsoleTests
{
    [Fact]
    public void CreateDefaultBuilder_ShouldEstablishConsoleWithEmptyRoot()
    {
        // User passed-in args
        var args = new string[] { "status", "test", "--optionOne", "testOption1" };

        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder(args)
                .AddCommand<TestCommand>("status")
                .AddCommand<TestCommand2>("status", "test");

        IAutomationIoConsole console = builder.Build();

        var resultCode = console.Run();

        Assert.True(resultCode >= 0);
    }

    [Fact]
    public void CreateDefaultBuilder_ShouldEstablishConsoleWithCommandRoot()
    {
        // User passed-in args
        var args = new string[] { "testing", "--optionOne", "testOption1" };

        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder<TestCommand>(args)
                .AddCommand<TestCommand2>("testing")
                .AddCommand<TestCommand3>("status", "test");

        IAutomationIoConsole console = builder.Build();

        var resultCode = console.Run();

        Assert.True(resultCode >= 0);
    }

    private class TestCommand : BaseCommand
    {
        private readonly string internalTestData;

        public TestCommand()
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

    private class TestCommand2 : TestCommand { }
    private class TestCommand3 : TestCommand { }
}
