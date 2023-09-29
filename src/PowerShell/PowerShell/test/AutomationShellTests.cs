// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Management.Automation;
using BlazorFocused.Automation.PowerShell.Test.TestBed.Services;
using BlazorFocused.Automation.PowerShell.Test.TestBed.Startup;
using BlazorFocused.Automation.PowerShell.Tools;

namespace BlazorFocused.Automation.PowerShell.Test;

public partial class AutomationShellTests
{
    [Fact]
    public void ShouldLoadDependency()
    {
        var testRuntimeServiceMock = new Mock<ITestRuntimeService>();

        using IAutomationCommand<TestIoCShell> context =
            AutomationSandbox.CreateContext<TestIoCShell, TestStartup>(
                serviceCollection =>
                {
                    serviceCollection.AddTransient(_ => testRuntimeServiceMock.Object);
                });

        context.RunCommand();

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
