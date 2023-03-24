﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoc.PSCmdlets.Integration.Services;
using AutomationIoc.PSCmdlets.Integration.Startup;
using AutomationIoc.PSCmdlets.Tools;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Management.Automation;
using Xunit;

namespace AutomationIoc.PSCmdlets;

public class IoCShellTests
{
    [Fact]
    public void ShouldLoadDependency()
    {
        var testServiceMock = new Mock<ITestSdkService>();

        using IAutomationCommand<TestIoCShell> context = AutomationSandbox.CreateContext<TestIoCShell, TestSDKStartup>(
            serviceCollection =>
            {
                serviceCollection.AddTransient(_ => testServiceMock.Object);
            });

        context.RunCommand();

        testServiceMock.Verify(service => service.RunMethod(), Times.Exactly(3));
    }

    [Cmdlet(VerbsData.Mount, "Test")]
    public class TestIoCShell : IoCShell<TestSDKStartup>
    {
        [AutomationDependency]
        protected ITestSdkService TestService { get; set; }

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