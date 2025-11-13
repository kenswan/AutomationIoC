// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test;

public class AutomationCommandExtensionsTests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void AddCommand_ShouldAddCommand(bool isRoot, bool hasInitializer)
    {
        const string expectedSubcommandName = "testCommand";
        const string expectedSubcommandDescription = "Test Command Description";

        IAutomationCommand automationCommand = isRoot
            ? new AutomationRootCommand(
                description: "Test Root Command",
                automationContext: new AutomationContext())
            : new AutomationCommand(
                name: "ParentCommandName",
                description: "Test Parent Command",
                automationContext: new AutomationContext());

        AutomationCommand subcommand = hasInitializer
            ? automationCommand.AddCommand<DynamicAutomationCommandInitializer>(
                name: expectedSubcommandName,
                description: expectedSubcommandDescription)
            : automationCommand.AddCommand(
                name: expectedSubcommandName,
                description: expectedSubcommandDescription);

        Assert.IsType<Command>(subcommand, exactMatch: false);

        Command actualParentCommand =
            Assert.IsType<Command>(automationCommand, exactMatch: false);

        Assert.Contains(subcommand, actualParentCommand.Children);

        Command actualSubcommand = Assert.Single(actualParentCommand.Subcommands);
        Assert.Equal(expectedSubcommandName, actualSubcommand.Name);
        Assert.Equal(expectedSubcommandDescription, actualSubcommand.Description);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void AddCommand_ShouldThrowExceptionWhenContextIsNull(bool isRoot, bool hasInitializer)
    {
        IAutomationContext automationContext = null;

        IAutomationCommand automationCommand = isRoot
            ? new AutomationRootCommand(
                description: "Test Root Command",
                automationContext: automationContext)
            : new AutomationCommand(
                name: "ParentCommandName",
                description: "Test Root Command",
                automationContext: automationContext);

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
        {
            AutomationCommand _ = hasInitializer
                ? automationCommand.AddCommand<DynamicAutomationCommandInitializer>(
                    name: "ShouldFail",
                    description: "This command should fail")
                : automationCommand.AddCommand(
                    name: "ShouldFail",
                    description: "This command should fail");
        });

        Assert.Equal("Parent Command is missing automation context.", exception.Message);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AddCommand_ShouldThrowExceptionWhenNotBasedOnCommandType(bool hasInitializer)
    {
        IAutomationCommand automationCommand = new EmptyAutomationCommand();

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
        {
            AutomationCommand _ = hasInitializer
                ? automationCommand.AddCommand<DynamicAutomationCommandInitializer>(
                    name: "ShouldFail",
                    description: "This command should fail")
                : automationCommand.AddCommand(
                    name: "ShouldFail",
                    description: "This command should fail");
        });

        Assert.Equal(
            expected: "Parent Command must inherit from System.CommandLine.Command to add subcommands.",
            actual: exception.Message);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AddCommand_ShouldInitializeNewCommandWithInitializer(bool isRoot)
    {
        const string expectedSubcommandName = "testCommand";
        const string expectedSubcommandDescription = "Test Command Description";

        IAutomationCommand automationCommand = isRoot
            ? new AutomationRootCommand(
                description: "Test Root Command",
                automationContext: new AutomationContext())
            : new AutomationCommand(
                name: "ParentCommandName",
                description: "Test Parent Command",
                automationContext: new AutomationContext());

        AutomationCommand subcommand =
            automationCommand.AddCommand<DynamicAutomationCommandInitializer>(
                name: expectedSubcommandName,
                description: expectedSubcommandDescription);

        Command actualParentCommand =
            Assert.IsType<Command>(automationCommand, exactMatch: false);

        Assert.Contains(subcommand, actualParentCommand.Children);
        Command actualSubcommand = Assert.Single(actualParentCommand.Subcommands);

        Argument argument = Assert.Single(actualSubcommand.Arguments);
        Option option = Assert.Single(actualSubcommand.Options);

        Assert.Equal(DynamicAutomationCommandInitializer.ArgumentName, argument.Name);
        Assert.Equal(DynamicAutomationCommandInitializer.ArgumentDescription, argument.Description);
        Assert.Equal(DynamicAutomationCommandInitializer.OptionName, option.Name);
        Assert.Equal(DynamicAutomationCommandInitializer.OptionDescription, option.Description);
    }

    private class EmptyAutomationCommand : IAutomationCommand
    {
        public IAutomationContext Context => new AutomationContext();
        public void Add(Argument argument) => throw new NotImplementedException();

        public void Add(Option option) => throw new NotImplementedException();

        public void SetAction(Action<ParseResult, IAutomationContext> action) =>
            throw new NotImplementedException();

        public void SetAction(Func<ParseResult, IAutomationContext, CancellationToken, Task> action) =>
            throw new NotImplementedException();
    }

    private class DynamicAutomationCommandInitializer : IAutomationCommandInitializer
    {
        public const string OptionName = "--testOption";
        public const string OptionDescription = "Description of a test option.";
        public const string ArgumentName = "testArgument";
        public const string ArgumentDescription = "Description of a test argument.";

        public bool CommandInitialized { get; private set; }
        public bool CommandWasCalled { get; private set; }

        public void Initialize(IAutomationCommand command)
        {
            Option<string> option = new(name: OptionName)
            {
                Description = OptionDescription
            };

            Argument<int> argument = new(ArgumentName)
            {
                Description = ArgumentDescription
            };

            command.Add(option);
            command.Add(argument);
            this.CommandInitialized = true;

            command.SetAction((parseResult, automationContext) => { this.CommandWasCalled = true; });
        }
    }
}
