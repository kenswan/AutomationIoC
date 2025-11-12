// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Test.TestBed.Commands;
using AutomationIoC.CommandLine.Test.TestBed.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.CommandLine.Test;

public partial class AutomationIoConsoleTests
{
    [Fact]
    public void Run_ShouldOnlyRegisterConfigurationAndServicesAtStartup()
    {
        // User passed-in args
        string[] args = ["testing", "--test", "this is a test param"];

        // Should not begin services here, would fail here otherwise
        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("This is a test", args)
                .AddCommand<TestServiceWithExceptionCommandInitializer>("testing");

        // Should not begin services here, would fail here otherwise
        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        // Should send failed results code with 1 or greater
        Assert.True(resultCode > 0);
    }

    [Fact]
    public void Run_ShouldOnlyRegisterConfigurationAndServicesOnceWithMultipleCommands()
    {
        // User passed-in args
        string[] args = ["target", "--test", "testing"];

        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("This is a test", args)
                .AddCommand<TestServiceWithExceptionCommandInitializer>("testing1")
                .AddCommand<TestServiceWithExceptionCommandInitializer>("testing2")
                .AddCommand<TestServiceWithExceptionCommandInitializer>("testing3")
                .AddCommand<TestServiceWithoutExceptionCommandInitializer>("target");

        builder.Configure((hostBuilderContext, configurationBuilder) =>
        {
            var appSettings = new Dictionary<string, string>
            {
                [TestService.CONFIG_KEY] = TestService.CONFIG_VALUE
            };

            configurationBuilder.AddInMemoryCollection(appSettings);
        });

        builder.ConfigureServices((hostBuilderContext, services) =>
            services.AddTransient<ITestService, TestService>());

        IAutomationConsole console = builder.Build();

        int resultCode = console.Run();

        // Should not encounter failure with targeted command (set action)
        // If other commands registered services too, it would fail like in previous test
        Assert.Equal(0, resultCode);
    }
}
