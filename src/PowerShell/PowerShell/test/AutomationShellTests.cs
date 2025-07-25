// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PowerShell.Test.TestBed.Services;
using AutomationIoC.PowerShell.Test.TestBed.Startup;
using AutomationIoC.PowerShell.Tools;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Management.Automation;

namespace AutomationIoC.PowerShell.Test;

public partial class AutomationShellTests
{
    [Fact]
    public void ShouldLoadDependency()
    {
        var testRuntimeServiceMock = new Mock<ITestRuntimeService>();

        using IPowerShellAutomation<TestStartup> context =
            AutomationSandbox.CreateSession<TestStartup>(
                serviceCollection =>
                {
                    serviceCollection.AddTransient(_ => testRuntimeServiceMock.Object);
                });

        context.RunAutomationCommand<TestIoCShell>();

        testRuntimeServiceMock.Verify(service => service.RunMethod(), Times.Exactly(3));
    }

    [Cmdlet(VerbsData.Mount, "Test")]
    public class TestIoCShell : AutomationShell<TestStartup>
    {
        [AutomationDependency]
        protected ITestRuntimeService TestRuntimeService { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            TestRuntimeService.RunMethod();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            TestRuntimeService.RunMethod();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();

            TestRuntimeService.RunMethod();
        }
    }
}
