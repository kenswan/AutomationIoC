// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.PSCmdlets.Integration.Services;
using AutomationIoC.PSCmdlets.Integration.Startup;
using AutomationIoC.PSCmdlets.Tools;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC.PSCmdlets;

public class IoCShellTests
{
    [Fact]
    public void ShouldLoadDependency()
    {
        var testServiceMock = new Mock<ITestPSCmdletService>();

        using IAutomationCommand<TestIoCShell> context = AutomationSandbox.CreateContext<TestIoCShell, TestPSCmdletStartup>(
            serviceCollection =>
            {
                serviceCollection.AddTransient(_ => testServiceMock.Object);
            });

        context.RunCommand();

        testServiceMock.Verify(service => service.RunMethod(), Times.Exactly(3));
    }

    [Cmdlet(VerbsData.Mount, "Test")]
    public class TestIoCShell : IoCShell<TestPSCmdletStartup>
    {
        [AutomationDependency]
        protected ITestPSCmdletService TestService { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            TestService.RunMethod();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            TestService.RunMethod();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();

            TestService.RunMethod();
        }
    }
}
