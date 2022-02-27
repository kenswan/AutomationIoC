﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutomationIoC.Services;
using System.Management.Automation;

namespace AutomationIoC.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Build, "Dependencies")]
    public class TestStartup : AutomationStartup
    {
        private readonly TestService testService;

        public TestStartup()
        {
            testService = new();
        }

        public int CallCount => testService.CallCount;

        public override void Configure(IConfigurationBuilder configurationBuilder)
        {
            _ = testService.CallTestMethod();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(_ => testService);

            _ = testService.CallTestMethod();
        }
    }
}