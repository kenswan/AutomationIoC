﻿using Microsoft.Extensions.DependencyInjection;
using AutomationIoC.Services;
using System.Management.Automation;

namespace AutomationIoC.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Test")]
    public class TestShell : IoCShell
    {
        public TestShell(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override void ExecuteCmdlet(IServiceProvider serviceProvider)
        {
            var testService = serviceProvider.GetRequiredService<TestService>();

            WriteVerbose($"From Test Service: {testService.CallTestMethod()}");
        }
    }
}