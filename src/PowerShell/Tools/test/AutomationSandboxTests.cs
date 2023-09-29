// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Commands;
using BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Services;
using BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Startup;

namespace BlazorFocused.Automation.PowerShell.Tools.Test;

public class AutomationSandboxTests
{
    [Fact]
    public void ShouldRunCommandContext()
    {
        using IAutomationCommand<TestContextCommand> context =
            AutomationSandbox.CreateContext<TestContextCommand, TestStartup>(services =>
        {
            services.AddTransient<ITestService, TestService>();
        });

        ICollection<PSObject> results = context.RunCommand();

        Assert.Single(results);
    }

    [Fact]
    public void ShouldRunCommand()
    {
        string expectedValue = Guid.NewGuid().ToString();

        using IAutomationCommand<TestCommand> command = AutomationSandbox.CreateCommand<TestCommand>();

        ICollection<PSObject> results = command.RunCommand(command => command.AddParameter("Test", expectedValue));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First());
    }
}
