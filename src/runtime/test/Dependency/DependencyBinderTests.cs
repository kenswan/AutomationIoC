// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Attributes;
using AutomationIoC.Runtime.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AutomationIoC.Runtime.Dependency;

public class DependencyBinderTests
{
    [Fact]
    public void ShouldBindAttributeFields()
    {
        IServiceProvider serviceProvider = GenerateServiceProvider();
        var dependencyBinder = new DependencyBinder(serviceProvider);
        var fieldInstance = new TestRuntimeFieldService();

        dependencyBinder.LoadFieldsByAttribute<TestRuntimeAttribute>(fieldInstance);

        fieldInstance.RunMethod();

        Assert.True(fieldInstance.WasCalled);
        Assert.Equal(1, fieldInstance.CallCount);
    }

    [Fact]
    public void ShouldBindAttributeProperties()
    {
        IServiceProvider serviceProvider = GenerateServiceProvider();
        var propertyInstance = new TestRuntimePropertyService();
        var dependencyBinder = new DependencyBinder(serviceProvider);

        dependencyBinder.LoadPropertiesByAttribute<TestRuntimeAttribute>(propertyInstance);

        propertyInstance.RunMethod();

        Assert.True(propertyInstance.WasCalled);
        Assert.Equal(1, propertyInstance.CallCount);
    }

    [Fact]
    public void ShouldThrowIfServiceProviderIsNull() => Assert.Throws<ArgumentNullException>(() => new DependencyBinder(null));

    private static IServiceProvider GenerateServiceProvider() => new ServiceCollection()
            .AddTransient<ITestRuntimeService, TestRuntimeFieldService>()
            .AddTransient<ITestRuntimeInternalServiceOne, TestRuntimeInternalServiceOne>()
            .AddTransient<ITestRuntimeInternalServiceTwo, TestRuntimeInternalServiceTwo>()
            .BuildServiceProvider();
}
