// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoc.Runtime.Attributes;
using AutomationIoc.Runtime.Models;
using AutomationIoc.Runtime.Services;
using AutomationIoc.Runtime.Startup;
using AutomationIoC.Runtime.Dependency;
using AutomationIoC.Runtime.Session;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace AutomationIoC.Runtime.Context;

public class ContextBuilderTests
{
    private readonly Mock<IAutomationEnvironment> environmentMock;
    private readonly TestRuntimeStartup startup;
    private readonly Mock<ISessionStorageProvider> storageProviderMock;

    private readonly IContextBuilder contextBuilder;

    public ContextBuilderTests()
    {
        environmentMock = new();
        startup = new();
        storageProviderMock = new();

        contextBuilder = new ContextBuilder(environmentMock.Object, startup, storageProviderMock.Object);
    }

    [Fact]
    public void ShouldBuildServices()
    {
        contextBuilder.BuildServices();

        startup.AutomationEnvironment.Should().BeEquivalentTo(environmentMock.Object);

        storageProviderMock.Verify(provider =>
            provider.StoreHostProvider(It.Is<IHost>(host =>
                ServiceProviderIsConfiguredFromStartup(host.Services))));
    }

    [Fact]
    public void ShouldBuildServicesWithCollection()
    {
        IServiceCollection serviceCollection = new ServiceCollection()
            .AddTransient<ITestRuntimeService, TestRuntimeService>()
            .AddTransient<ITestRuntimeInternalServiceOne, TestRuntimeInternalServiceOne>()
            .AddTransient<ITestRuntimeInternalServiceTwo, TestRuntimeInternalServiceTwo>();

        contextBuilder.BuildServices(serviceCollection);

        storageProviderMock.Verify(provider =>
            provider.StoreHostProvider(It.Is<IHost>(host =>
                ServiceProviderIsConfiguredFromCollection(host.Services))));
    }

    [Fact]
    public void ShouldInitializeInstance()
    {
        var dependencyBinderMock = new Mock<IDependencyBinder>();
        var instance = new TestInstance(2);

        IServiceProvider serviceProvider =
            new ServiceCollection()
                .AddTransient(_ => dependencyBinderMock.Object).BuildServiceProvider();

        storageProviderMock.Setup(provider =>
            provider.GetCurrentServiceProvider()).Returns(serviceProvider);

        contextBuilder.InitializeCurrentInstance<TestRuntimeAttribute>(instance);

        dependencyBinderMock.Verify(binder =>
            binder.LoadFieldsByAttribute<TestRuntimeAttribute>(instance), Times.Once());

        dependencyBinderMock.Verify(binder =>
            binder.LoadPropertiesByAttribute<TestRuntimeAttribute>(instance), Times.Once());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldReportInitializationStatus(bool foundServiceProvider)
    {
        IServiceProvider serviceProvider = foundServiceProvider ?
            new ServiceCollection().BuildServiceProvider() : null;

        storageProviderMock.Setup(provider => provider.GetCurrentServiceProvider()).Returns(serviceProvider);

        Assert.Equal(foundServiceProvider, contextBuilder.IsInitialized);
    }

    private static bool ServiceProviderIsConfiguredFromStartup(IServiceProvider serviceProvider)
    {
        IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
        IDependencyBinder dependencyBinder = serviceProvider.GetService<IDependencyBinder>();
        ITestRuntimeService testService = serviceProvider.GetService<ITestRuntimeService>();

        return configuration.GetValue<string>(TestRuntimeStartup.CONFIGURATION_KEY) == TestRuntimeStartup.CONFIGURATION_VALUE &&
            dependencyBinder is not null &&
            testService is not null;
    }

    private static bool ServiceProviderIsConfiguredFromCollection(IServiceProvider serviceProvider)
    {
        IDependencyBinder dependencyBinder = serviceProvider.GetService<IDependencyBinder>();
        ITestRuntimeService service = serviceProvider.GetService<ITestRuntimeService>();
        ITestRuntimeInternalServiceOne serviceOne = serviceProvider.GetService<ITestRuntimeInternalServiceOne>();
        ITestRuntimeInternalServiceTwo serviceTwo = serviceProvider.GetService<ITestRuntimeInternalServiceTwo>();

        return dependencyBinder is not null &&
            service is not null &&
            serviceOne is not null &&
            serviceTwo is not null;
    }
}
