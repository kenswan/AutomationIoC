﻿using AutomationIoC.Sample.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;

namespace AutomationIoC.Sample
{
    [Cmdlet(VerbsLifecycle.Start, "Game")]
    public class Startup : IoCStartup
    {
        public override void Configure(IConfigurationBuilder configurationBuilder)
        {
            var appSettings = new Dictionary<string, string>()
            {
                ["player:mode"] = "Beginner",
            };

            configurationBuilder.AddInMemoryCollection(appSettings);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Deck>();
        }
    }
}
