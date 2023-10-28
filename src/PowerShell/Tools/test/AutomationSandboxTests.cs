// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Services;
using BlazorFocused.Automation.PowerShell.Tools.Test.TestBed.Startup;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace BlazorFocused.Automation.PowerShell.Tools.Test;

public class AutomationSandboxTests
{
    [Fact]
    public void ShouldRunCommandContext()
    {
        using IPowerShellAutomation<TestStartup> context =
            AutomationSandbox.CreateSession<TestStartup>(services =>
            {
                services.AddTransient<ITestService, TestService>();
            });

        ICollection<PSObject> results = context.RunCommand("Test-Context");

        Assert.Single(results);
    }

    [Fact]
    public void ShouldRunCommand()
    {
        string expectedValue = Guid.NewGuid().ToString();

        using IPowerShellAutomation<TestStartup> command = AutomationSandbox.CreateSession<TestStartup>();

        ICollection<PSObject> results =
            command.RunCommand("Test-Context", command => command.AddParameter("Test", expectedValue));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First());
    }
}
