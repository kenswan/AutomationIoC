// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Tools.Context;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Commands;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Services;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Startup;
using AutomationIoC.Runtime;
using Bogus;
using Moq;
using System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools.Test.Context;

public partial class PowerShellAutomationContextTests
{
    [Fact]
    public void RunAutomationCommand_ShouldRunWithDependencyInjection()
    {
        int expectedCallCount = new Faker().Random.Int(5, 10);

        using var powerShellAutomationContext = new PowerShellAutomationContext<TestStartup>();

        ICollection<PSObject> results =
            powerShellAutomationContext.RunAutomationCommand<TestDependencyCommand>(command =>
                command.AddParameter("Times", expectedCallCount));

        Assert.Single(results);
        Assert.Equal(expectedCallCount, Convert.ToInt64(results.First().BaseObject));
    }

    [Fact]
    public void RunAutomationCommand_ShouldOverrideWithCustomInjectedServices()
    {
        var testServiceMock = new Mock<ITestService>();
        int originalCallCount = new Faker().Random.Int(5, 10);
        int mockedCallCount = new Faker().Random.Int(50, 100);

        testServiceMock.Setup(service => service.CallTestMethod()).Verifiable();
        testServiceMock.SetupGet(service => service.CallCount).Returns(mockedCallCount);

        using var powerShellAutomationContext =
            new PowerShellAutomationContext<TestStartup>(services =>
            {
                services.ReplaceRegisteredService<ITestService>((_) => testServiceMock.Object);
            });

        ICollection<PSObject> results =
            powerShellAutomationContext.RunAutomationCommand<TestDependencyCommand>(command =>
                command.AddParameter("Times", originalCallCount));

        Assert.Single(results);
        Assert.Equal(mockedCallCount, Convert.ToInt64(results.First().BaseObject));
    }
}
