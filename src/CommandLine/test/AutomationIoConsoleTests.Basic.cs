// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Commands;

namespace AutomationIoC.CommandLine.Test;

public partial class AutomationIoConsoleTests
{
    [Fact]
    public void CreateDefaultBuilder_ShouldEstablishConsoleWithEmptyRoot()
    {
        // User passed-in args
        string[] args = new string[] { "status", "test", "--optionOne", "testOption1" };

        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("This is a test", args)
                .AddCommand<BasicTestCommand>("status")
                .AddCommand<BasicTestCommand2>("status", "test");

        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        Assert.True(resultCode == 0);
    }

    [Fact]
    public void CreateDefaultBuilder_ShouldEstablishConsoleWithCommandRoot()
    {
        // User passed-in args
        string[] args = new string[] { "testing", "--optionOne", "testOption1" };

        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder<BasicTestCommand>("This is a test", args)
                .AddCommand<BasicTestCommand2>("testing")
                .AddCommand<BasicTestCommand3>("status", "test");

        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        Assert.True(resultCode == 0);
    }

    [Fact]
    public void CreateDefaultBuilder_ShouldFailIfCommandNotRegistered()
    {
        // User passed-in args
        string[] args = new string[] { "not-registered", "--optionOne", "testOption1" };

        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder<BasicTestCommand>("This is a test", args)
                .AddCommand<BasicTestCommand2>("testing")
                .AddCommand<BasicTestCommand3>("status", "test");

        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        // Failure result codes will be greater than 0
        Assert.True(resultCode > 0);
    }
}
