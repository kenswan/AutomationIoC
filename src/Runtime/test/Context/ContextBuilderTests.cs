// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.Runtime.Context;
using BlazorFocused.Automation.Runtime.Test.TestBed.Services;
using BlazorFocused.Automation.Runtime.Test.TestBed.Startup;
using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace BlazorFocused.Automation.Runtime.Test.Context;

public class ContextBuilderTests
{
    private readonly TestHostBuildContextStartup startup;
    private readonly Mock<ISessionStorage> sessionStorageMock;

    private readonly string hostKey = typeof(TestHostBuildContextStartup).FullName;

    private readonly IContextBuilder contextBuilder;

    public enum BuildServiceTypes { BuildServicesEmpty, BuildServicesPopulated, GetCurrentServiceProvider };

    public ContextBuilderTests()
    {
        startup = GenerateTestHostBuildContextStartup();
        sessionStorageMock = new();

        contextBuilder = new ContextBuilder(startup, sessionStorageMock.Object);
    }

    [Fact]
    public void BuildServices_ShouldBuildStartupServices()
    {
        // Act
        IServiceProvider serviceProvider = contextBuilder.BuildServices();

        // Assert
        sessionStorageMock.Verify(provider =>
            provider.SetValue(hostKey,
                It.Is<IHost>(host =>
                    // Verify Stored Host
                    VerifyServiceProviderIsConfiguredFromHostContextStartup(host.Services))),
                        Times.Once());

        // Verify Actual provider returned
        VerifyServiceProviderIsConfiguredFromHostContextStartup(serviceProvider);
    }

    [Fact]
    public void BuildServices_ShouldBuildInjectedServicesWithCollection()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection()
            .AddTransient<IOutsideServiceDependency, OutsideServiceDependency>();

        // Act
        IServiceProvider serviceProvider = contextBuilder.BuildServices(serviceCollection);

        // Assert
        sessionStorageMock.Verify(storage =>
            storage.SetValue(hostKey,
                It.Is<IHost>(host =>
                    // Verify Stored Host
                    VerifyServiceProviderIsConfiguredFromCollection(host.Services))),
                        Times.Once());

        // Verify Actual provider returned
        VerifyServiceProviderIsConfiguredFromCollection(serviceProvider);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsInitialized_ShouldReportInitializationStatus(bool expectedIsHostInitialized)
    {
        // Arrange
        using IHost host = expectedIsHostInitialized ?
            new HostBuilder().Build() : null;

        sessionStorageMock.Setup(storage =>
            storage.GetValue<IHost>(hostKey))
                .Returns(host);

        // Act
        bool actualIsHostInitialized = contextBuilder.IsInitialized;

        // Assert
        Assert.Equal(expectedIsHostInitialized, actualIsHostInitialized);

        sessionStorageMock.Verify(provider =>
            provider.GetValue<IHost>(hostKey),
                Times.Once());
    }

    [Theory]
    [InlineData(BuildServiceTypes.BuildServicesEmpty)]
    [InlineData(BuildServiceTypes.BuildServicesPopulated)]
    [InlineData(BuildServiceTypes.GetCurrentServiceProvider)]
    public void IsInitialized_ShouldNotCheckStorageForInitializedInScope(BuildServiceTypes method)
    {
        // Arrange
        using IHost host = new HostBuilder().Build();

        sessionStorageMock.Setup(storage =>
            storage.GetValue<IHost>(hostKey))
                .Returns(null as IHost);

        // "GetValue" method gets called in "GetCurrentServiceProvider" method
        // So call should happen at least once in those cases, but never on
        // "IsInitialized"
        Times expectedTimesGetValueMethodCalled =
            method == BuildServiceTypes.GetCurrentServiceProvider ?
                Times.Once() : Times.Never();

        // Act

        // These methods should initialize storage
        IServiceProvider _ = method switch
        {
            BuildServiceTypes.BuildServicesEmpty => contextBuilder.BuildServices(),
            BuildServiceTypes.BuildServicesPopulated => contextBuilder.BuildServices(new ServiceCollection()),
            BuildServiceTypes.GetCurrentServiceProvider => contextBuilder.GetContextServiceProvider(),
            _ => throw new InvalidOperationException()
        };

        bool actualIsHostInitialized = contextBuilder.IsInitialized;

        // Assert
        Assert.True(actualIsHostInitialized);

        sessionStorageMock.Verify(provider =>
            provider.GetValue<IHost>(hostKey),
                expectedTimesGetValueMethodCalled);
    }

    [Fact]
    public void GetContextServiceProvider_ShouldBuildIfCachedHostNotAvailable()
    {
        // Arrange
        sessionStorageMock.Setup(storage =>
            storage.GetValue<IHost>(hostKey))
                .Returns(null as IHost);

        // Act
        IServiceProvider serviceProvider = contextBuilder.GetContextServiceProvider();

        // Assert
        sessionStorageMock.Verify(provider =>
            provider.SetValue(hostKey,
                It.Is<IHost>(host =>
                    // Verify Stored Host
                    VerifyServiceProviderIsConfiguredFromHostContextStartup(host.Services))),
                        Times.Once());

        sessionStorageMock.Verify(storage =>
            storage.GetValue<IHost>(hostKey),
                Times.Once);

        // Verify Actual provider returned
        VerifyServiceProviderIsConfiguredFromHostContextStartup(serviceProvider);
    }

    [Fact]
    public void GetContextServiceProvider_ShouldNotBuildIfCachedHostIsAvailable()
    {
        // Arrange
        using IHost host = new HostBuilder().Build();

        sessionStorageMock.Setup(storage =>
            storage.GetValue<IHost>(hostKey))
                .Returns(host);

        // Act
        IServiceProvider serviceProvider = contextBuilder.GetContextServiceProvider();

        // Assert
        sessionStorageMock.Verify(storage =>
            storage.GetValue<IHost>(hostKey),
                Times.Once);

        sessionStorageMock.VerifyNoOtherCalls();
    }

    private static bool VerifyServiceProviderIsConfiguredFromHostContextStartup(IServiceProvider serviceProvider)
    {
        IConfiguration configuration = serviceProvider.GetService<IConfiguration>();

        string environmentName =
            serviceProvider.GetRequiredService<IHostEnvironment>().EnvironmentName;

        TestHostBuilderContextService testHostBuilderContextService =
            serviceProvider.GetService<TestHostBuilderContextService>();

        ITestRuntimeService service = serviceProvider.GetService<ITestRuntimeService>();

        ITestRuntimeInternalServiceOne serviceOne =
            serviceProvider.GetService<ITestRuntimeInternalServiceOne>();

        ITestRuntimeInternalServiceTwo serviceTwo =
            serviceProvider.GetService<ITestRuntimeInternalServiceTwo>();

        return
            configuration is not null &&
            environmentName == TestHostBuildContextStartup.ENVIRONMENT_NAME &&
            testHostBuilderContextService is not null &&
            service is not null &&
            serviceOne is not null &&
            serviceTwo is not null;
    }

    private static bool VerifyServiceProviderIsConfiguredFromCollection(IServiceProvider serviceProvider)
    {
        IOutsideServiceDependency outsideServiceDependency =
            serviceProvider.GetService<IOutsideServiceDependency>();

        return VerifyServiceProviderIsConfiguredFromHostContextStartup(serviceProvider) &&
            outsideServiceDependency is not null;
    }

    private static TestHostBuildContextStartup GenerateTestHostBuildContextStartup(
        string connectionString = null,
        string configurationKey = null,
        string configurationValue = null)
    {
        connectionString ??= new Faker().Internet.Password();
        configurationKey ??= "IPAddress";
        configurationValue ??= new Faker().Internet.Ipv6();

        return new TestHostBuildContextStartup(
            connectionString: connectionString,
            configurationCheckKey: configurationKey,
            configurationCheckValue: configurationValue);
    }

    private interface IOutsideServiceDependency
    {
        string Name { get; }
    }

    private class OutsideServiceDependency : IOutsideServiceDependency
    {
        public string Name { get; } = "ThisIsATest";
    }
}
