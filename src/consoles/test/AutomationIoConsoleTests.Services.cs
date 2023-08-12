// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace AutomationIoC.Consoles;

public partial class AutomationIoConsoleTests
{
    private const string CONFIG_KEY = "TestConfiguration";
    private const string CONFIG_VALUE = "ExecutedConfiguration";

    [Fact]
    public void Run_ShouldOnlyRegisterConfigurationAndServicesAtStartup()
    {
        // User passed-in args
        string[] args = new string[] { "testing", "--test", "this iss a test param" };

        // Should not begin services here, would fail here otherwise
        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder(args)
                .AddCommand<TestServiceWithExceptionCommand>("testing");

        // Should not begin services here, would fail here otherwise
        IAutomationIoConsole console = builder.Build();

        int resultCode = console.Run();

        // Should send failed results code with 1 or greater
        Assert.True(resultCode > 0);
    }

    [Fact]
    public void Run_ShouldOnlyRegisterConfigurationAndServicesOnceWithMultipleCommands()
    {
        // User passed-in args
        string[] args = new string[] { "target", "--test", "testing" };

        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder(args)
                .AddCommand<TestServiceWithExceptionCommand>("testing1")
                .AddCommand<TestServiceWithExceptionCommand>("testing2")
                .AddCommand<TestServiceWithExceptionCommand>("testing3")
                .AddCommand<TestServiceWithoutExceptionCommand>("target");

        IAutomationIoConsole console = builder.Build();

        int resultCode = console.Run();

        // Should not encounter failure with targeted command
        // If other commands registered services too, it would fail like in previous tests
        Assert.True(resultCode == 0);
    }

    private class TestServiceWithoutExceptionCommand : StandardCommand
    {

        public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
        {
            var passedInOption = new Option<string>(
                name: "--test",
                description: "Description of test.");

            command.AddOption(passedInOption);

            command.SetHandler(TestExecution,
                serviceBinderFactory.Bind<ITestService>(),
                passedInOption);
        }

        public override Action<IConfigurationBuilder> ConfigurationBuilder => (configurationBuilder) =>
        {
            var appSettings = new Dictionary<string, string>()
            {
                [CONFIG_KEY] = CONFIG_VALUE,
            };

            configurationBuilder.AddInMemoryCollection(appSettings);
        };

        public override Action<IServiceCollection> Services => (serviceCollection) =>
            serviceCollection.AddTransient<ITestService, TestService>();

        private static void TestExecution(ITestService testService, string data) => testService.Execute(data);
    }

    private class TestServiceWithExceptionCommand : StandardCommand
    {

        public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
        {
            var passedInOption = new Option<string>(
                name: "--test",
                description: "Description of test.");

            command.AddOption(passedInOption);

            command.SetHandler(TestExecution,
                serviceBinderFactory.Bind<ITestService>(),
                passedInOption);
        }

        public override Action<IConfigurationBuilder> ConfigurationBuilder => (configurationBuilder) =>
        {
            var appSettings = new Dictionary<string, string>()
            {
                [CONFIG_KEY] = CONFIG_VALUE,
            };

            configurationBuilder.AddInMemoryCollection(appSettings);

            throw new NotImplementedException();
        };

        public override Action<IServiceCollection> Services => (serviceCollection) =>
        {
            serviceCollection.AddTransient<ITestService, TestService>();

            throw new NotImplementedException();
        };

        private static void TestExecution(ITestService testService, string data) => testService.Execute(data);
    }

    private interface ITestService
    {
        public void Execute(string data);
    }

    private class TestService : ITestService
    {
        public TestService(IConfiguration configuration)
        {
            string configurationValue = configuration.GetValue<string>(CONFIG_KEY);

            if (configurationValue != CONFIG_VALUE)
            {
                throw new Exception("Test Service is not configured properly");
            }
        }

        public void Execute(string data) => Console.WriteLine("Executed Data:{0}", data);
    }
}
