// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Tools.Test.TestBed.Commands;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Services;
using AutomationIoC.PowerShell.Tools.Test.TestBed.Startup;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Management.Automation;

namespace AutomationIoC.PowerShell.Tools.Test;

public class AutomationSandboxTests
{
    [Fact]
    public void CreateSession_ShouldCreateGeneralPowerShellSession()
    {
        string environmentKey = Guid.NewGuid().ToString();
        string expectedValue = Guid.NewGuid().ToString();

        using IPowerShellAutomation powerShellAutomation = AutomationSandbox.CreateSession();

        powerShellAutomation.RunCommand("Set-Variable", command =>
            command
                .AddParameter("Name", environmentKey)
                .AddParameter("Value", expectedValue));

        ICollection<PSObject> results = powerShellAutomation.RunCommand("Get-Variable", command =>
            command.AddParameter("Name", environmentKey));

        Assert.Single(results);
    }

    [Fact]
    public void CreateSession_ShouldCreateGeneralPowerShellSessionForStandardPSCmdlet()
    {
        string inputParameter = new Faker().Random.AlphaNumeric(10);
        string expectedValue = TestPSCmdletCommand.TransformOutput(inputParameter);

        using IPowerShellAutomation powerShellAutomation = AutomationSandbox.CreateSession();

        ICollection<string> results = powerShellAutomation.RunCommand<TestPSCmdletCommand, string>(command =>
            command.AddParameter("Test", inputParameter));

        Assert.Single(results);
        Assert.Equal(expectedValue, results.First());
    }

    [Fact]
    public void CreateSession_ShouldCreateSessionWithDependencyStartup()
    {
        int expectedCallCount = new Faker().Random.Int(3, 10);

        using IPowerShellAutomation<TestStartup> powerShellAutomation = AutomationSandbox.CreateSession<TestStartup>();

        ICollection<int> results = powerShellAutomation.RunCommand<TestDependencyCommand, int>(command =>
            command.AddParameter("Times", expectedCallCount));

        Assert.Single(results);
        Assert.Equal(expectedCallCount, results.First());
    }

    [Fact]
    public void CreateSession_ShouldCreateSessionWithDependencyOverride()
    {
        var testServiceMock = new Mock<ITestService>();
        int originalCallCount = new Faker().Random.Int(5, 10);
        int mockedCallCount = new Faker().Random.Int(50, 100);

        testServiceMock.Setup(service => service.CallTestMethod()).Verifiable();
        testServiceMock.SetupGet(service => service.CallCount).Returns(mockedCallCount);

        using IPowerShellAutomation<TestStartup> powerShellAutomation =
            AutomationSandbox.CreateSession<TestStartup>(services =>
            {
                // Remove previous service registration
                ServiceDescriptor serviceDescriptor =
                    services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ITestService));

                if (serviceDescriptor is not null)
                {
                    services.Remove(serviceDescriptor);
                }

                // Add new mock in original service's place
                services.AddTransient(_ => testServiceMock.Object);
            });

        ICollection<int> results = powerShellAutomation.RunCommand<TestDependencyCommand, int>(command =>
            command.AddParameter("Times", originalCallCount));

        Assert.Single(results);
        Assert.Equal(mockedCallCount, results.First());
    }
}
