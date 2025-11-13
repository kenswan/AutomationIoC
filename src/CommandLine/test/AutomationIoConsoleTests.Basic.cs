// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Commands;
using AutomationIoC.Runtime;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test;

public partial class AutomationIoConsoleTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void CreateDefaultBuilder_ShouldEstablishConsoleWithEmptyRoot()
    {
        // User passed-in args
        string[] args = ["status", "test", "--optionOne", "testOption1"];

        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("This is a test", args)
                .AddCommand<BasicTestCommandInitializer>("status")
                .AddCommand<BasicTestCommand2>("status", "test");

        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        Assert.Equal(0, resultCode);
    }

    [Fact]
    public void CreateDefaultBuilder_ShouldEstablishConsoleWithCommandRoot()
    {
        // User passed-in args
        string[] args = ["testing", "--optionOne", "testOption1"];

        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder<BasicTestCommandInitializer>("This is a test", args)
                .AddCommand<BasicTestCommand2>("testing")
                .AddCommand<BasicTestCommand3>("status", "test");

        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        Assert.Equal(0, resultCode);
    }

    [Fact]
    public void CreateDefaultBuilder_ShouldFailIfCommandNotRegistered()
    {
        // User passed-in args
        string[] args = ["not-registered", "--optionOne", "testOption1"];

        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder<BasicTestCommandInitializer>("This is a test", args)
                .AddCommand<BasicTestCommand2>("testing")
                .AddCommand<BasicTestCommand3>("status", "test");

        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        // Failure result codes will be greater than 0
        Assert.True(resultCode > 0);
    }

    [Fact]
    public void CreateRootCommand_ShouldCreateStandaloneRootCommandForManualConfiguration()
    {
        string[] args = ["--optionOne", "testOption1"];
        IAutomationContext context = new AutomationContext();

        AutomationRootCommand rootCommand =
            AutomationConsole.CreateRootCommand<BasicTestCommandInitializer>(
                automationContext: context,
                appDescription: "Test Application");

        ParseResult result = rootCommand.Parse(args);

        int resultCode = result.Invoke();

        Assert.Equal(0, resultCode);
    }

    [Fact]
    public void CreateRootCommand_ShouldCreateStandaloneRootCommandWithoutConfiguration()
    {
        const string expectedOptionValue = "testOption1";
        string[] args = ["--optionOne", expectedOptionValue];
        string actualOptionOneValue = string.Empty;
        IAutomationContext context = new AutomationContext();

        AutomationRootCommand rootCommand =
            AutomationConsole.CreateRootCommand(
                automationContext: context,
                appDescription: "Test Application");

        Option<string> optionOne = new(name: "--optionOne")
        {
            Description = "Description of option one field."
        };
        rootCommand.Options.Add(optionOne);
        rootCommand.SetAction(parseResult =>
        {
            string optionOneValue = parseResult.GetValue(optionOne);
            testOutputHelper.WriteLine(optionOneValue);
            actualOptionOneValue = optionOneValue;
        });

        ParseResult result = rootCommand.Parse(args);

        int resultCode = result.Invoke();

        Assert.Equal(0, resultCode);
        Assert.Equal(expectedOptionValue, actualOptionOneValue);
    }
}
