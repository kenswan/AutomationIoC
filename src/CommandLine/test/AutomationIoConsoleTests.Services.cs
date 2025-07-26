// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Commands;

namespace AutomationIoC.CommandLine.Test;

public partial class AutomationIoConsoleTests
{
    [Fact]
    public void Run_ShouldOnlyRegisterConfigurationAndServicesAtStartup()
    {
        // User passed-in args
        string[] args = ["testing", "--test", "this is a test param"];

        // Should not begin services here, would fail here otherwise
        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("This is a test", args)
                .AddCommand<TestServiceWithExceptionCommand>("testing");

        // Should not begin services here, would fail here otherwise
        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        // Should send failed results code with 1 or greater
        Assert.True(resultCode > 0);
    }

    [Fact]
    public void Run_ShouldOnlyRegisterConfigurationAndServicesOnceWithMultipleCommands()
    {
        // User passed-in args
        string[] args = ["target", "--test", "testing"];

        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("This is a test", args)
                .AddCommand<TestServiceWithExceptionCommand>("testing1")
                .AddCommand<TestServiceWithExceptionCommand>("testing2")
                .AddCommand<TestServiceWithExceptionCommand>("testing3")
                .AddCommand<TestServiceWithoutExceptionCommand>("target");

        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        // Should not encounter failure with targeted command
        // If other commands registered services too, it would fail like in previous test
        Assert.Equal(0, resultCode);
    }
}
