﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerShellFocused.Services;
using System.Management.Automation;

namespace PowerShellFocused.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Build, "Dependencies")]
    public class TestStartup : FocusedStartup
    {
        private readonly TestService testService;

        public TestStartup()
        {
            testService = new();
        }

        public IServiceProvider InternalServiceProvider => ServiceProvider;

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
