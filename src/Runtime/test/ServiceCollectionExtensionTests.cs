// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Test.TestBed.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Test;

public class ServiceCollectionExtensionTests
{
    [Theory]
    [InlineData(ServiceLifetime.Transient)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Singleton)]
    public void ReplaceRegisteredService_ShouldReplaceRegisteredService(ServiceLifetime serviceLifetime)
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        Type serviceType = typeof(ITestService);

        var originalInstance = new TestService();
        var replacementInstance = new TestService();

        IServiceCollection _ = serviceLifetime switch
        {
            ServiceLifetime.Transient => serviceCollection.AddTransient(serviceType, (_) => originalInstance),
            ServiceLifetime.Scoped => serviceCollection.AddScoped(serviceType, (_) => originalInstance),
            ServiceLifetime.Singleton => serviceCollection.AddSingleton(serviceType, originalInstance),
            _ => throw new NotImplementedException($"Lifetime {serviceLifetime} not supported for removal")
        };

        ITestService originalServiceRegistration = serviceCollection.BuildServiceProvider().GetService<ITestService>();

        // Act
        serviceCollection.ReplaceRegisteredService<ITestService>((_) => replacementInstance);

        // Assert
        Assert.NotNull(originalServiceRegistration);
        Assert.Equal(originalInstance.Id, ((TestService)originalServiceRegistration).Id);

        ITestService replacementServiceRegistration = serviceCollection.BuildServiceProvider().GetService<ITestService>();

        Assert.NotNull(replacementServiceRegistration);
        Assert.Equal(replacementInstance.Id, ((TestService)replacementServiceRegistration).Id);
    }

    [Fact]
    public void ReplaceRegisteredService_ShouldThrowInvalidOperationWhenNotRegistered()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        Type serviceType = typeof(ITestService);

        var unregisteredInstance = new TestService();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => serviceCollection.ReplaceRegisteredService<ITestService>((_) => unregisteredInstance));
    }
}
