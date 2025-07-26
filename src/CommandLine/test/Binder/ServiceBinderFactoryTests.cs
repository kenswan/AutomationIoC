// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine.Binder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.Binder;

public class ServiceBinderFactoryTests
{
    [Fact]
    public void Bind_ShouldConfigureServices()
    {
        // Arrange
        var serviceBinderFactoryTestCommand = new ServiceBinderFactoryTestCommand();
        var serviceBinderFactory = new ServiceBinderFactory(serviceBinderFactoryTestCommand);

        // Act
        ServiceBinderFactoryTestService service =
            serviceBinderFactory.Bind<ServiceBinderFactoryTestService>();

        // Assert
        Assert.NotNull(service);
        Assert.True(serviceBinderFactoryTestCommand.GenerateParametersCalled);
        Assert.True(serviceBinderFactoryTestCommand.GenerateParameterConfigurationMappingCalled);
        Assert.True(serviceBinderFactoryTestCommand.ConfigureCalled);
        Assert.True(serviceBinderFactoryTestCommand.ConfigureServicesCalled);
        Assert.Equal("TestValue", service.GetValueFromConfiguration("TestKey"));
    }

    private class ServiceBinderFactoryTestService(IConfiguration configuration)
    {
        public string GetValueFromConfiguration(string key) => configuration.GetValue<string>(key);
    }

    private class ServiceBinderFactoryTestCommand : IConsoleCommand
    {
        public bool GenerateParametersCalled { get; private set; }
        public bool GenerateParameterConfigurationMappingCalled { get; private set; }
        public bool ConfigureCalled { get; private set; }
        public bool ConfigureServicesCalled { get; private set; }

        public void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command) =>
            throw new NotImplementedException();

        public void Configure(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            this.ConfigureCalled = true;

            var appSettings = new Dictionary<string, string>
            {
                ["TestKey"] = "TestValue"
            };

            configurationBuilder.AddInMemoryCollection(appSettings);
        }

        public void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            this.ConfigureServicesCalled = true;

            services.AddTransient<ServiceBinderFactoryTestService>();
        }

        public string[] GenerateParameters()
        {
            this.GenerateParametersCalled = true;

            return [];
        }

        public IDictionary<string, string> GenerateParameterConfigurationMapping()
        {
            this.GenerateParameterConfigurationMappingCalled = true;

            return new Dictionary<string, string>();
        }
    }
}
