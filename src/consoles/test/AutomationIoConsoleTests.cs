// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.Consoles;

public class AutomationIoConsoleTests
{
    [Fact(Skip = "Validating API structure")]
    public void CreateDefaultBuilder_ShouldEstablishConsole()
    {
        var args = new string[] { "--option1", "testOption1" };
        IAutomationIoConsoleBuilder builder = AutomationIoConsole.CreateDefaultBuilder("TestApp");

        builder.AddCommand<TestCommand>("status");

        IAutomationIoConsole console = builder.Build();

        var resultCode = console.Run(args);

        Assert.True(resultCode > 0);
    }

    private class TestCommand : ActionCommand
    {
        public override void ConfigureCommand(Action<Command> configure) => throw new NotImplementedException();
    }
}
