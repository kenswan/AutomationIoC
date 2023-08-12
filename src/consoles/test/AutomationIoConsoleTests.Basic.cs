// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.Consoles;

public partial class AutomationIoConsoleTests
{
    [Fact]
    public void CreateDefaultBuilder_ShouldEstablishConsoleWithEmptyRoot()
    {
        // User passed-in args
        string[] args = new string[] { "status", "test", "--optionOne", "testOption1" };

        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder(args)
                .AddCommand<BasicTestCommand>("status")
                .AddCommand<BasicTestCommand2>("status", "test");

        IAutomationIoConsole console = builder.Build();

        int resultCode = console.Run();

        Assert.True(resultCode == 0);
    }

    [Fact]
    public void CreateDefaultBuilder_ShouldEstablishConsoleWithCommandRoot()
    {
        // User passed-in args
        string[] args = new string[] { "testing", "--optionOne", "testOption1" };

        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder<BasicTestCommand>(args)
                .AddCommand<BasicTestCommand2>("testing")
                .AddCommand<BasicTestCommand3>("status", "test");

        IAutomationIoConsole console = builder.Build();

        int resultCode = console.Run();

        Assert.True(resultCode == 0);
    }

    [Fact]
    public void CreateDefaultBuilder_ShouldFailIfCommandNotRegistered()
    {
        // User passed-in args
        string[] args = new string[] { "not-registered", "--optionOne", "testOption1" };

        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder<BasicTestCommand>(args)
                .AddCommand<BasicTestCommand2>("testing")
                .AddCommand<BasicTestCommand3>("status", "test");

        IAutomationIoConsole console = builder.Build();

        int resultCode = console.Run();

        // Failure result codes will be greater than 0
        Assert.True(resultCode > 0);
    }

    private class BasicTestCommand : StandardCommand
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

    private class BasicTestCommand2 : BasicTestCommand { }
    private class BasicTestCommand3 : BasicTestCommand { }
}
