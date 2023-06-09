// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Integration.Commands;
using AutomationIoC.PSCmdlets.Integration.Services;
using AutomationIoC.PSCmdlets.Integration.Startup;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC.PSCmdlets.Tools;

public class AutomationSandboxTests
{
    [Fact]
    public void ShouldRunCommandContext()
    {
        using IAutomationCommand<TestContextCommand> context = AutomationSandbox.CreateContext<TestContextCommand, TestStartup>(services =>
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
