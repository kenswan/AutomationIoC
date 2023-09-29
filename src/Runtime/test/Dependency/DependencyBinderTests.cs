// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using BlazorFocused.Automation.Runtime.Dependency;
using BlazorFocused.Automation.Runtime.Test.TestBed.Attributes;
using BlazorFocused.Automation.Runtime.Test.TestBed.Services;

namespace BlazorFocused.Automation.Runtime.Test.Dependency;

public class DependencyBinderTests
{
    [Fact]
    public void BindServicesByAttribute_ShouldBindAttributeFields()
    {
        IServiceProvider serviceProvider = GenerateServiceProvider();
        var fieldInstance = new TestRuntimeFieldService();

        DependencyBinder.BindServicesByAttribute<TestRuntimeAttribute>(serviceProvider, fieldInstance);

        fieldInstance.RunMethod();

        Assert.True(fieldInstance.WasCalled);
        Assert.Equal(1, fieldInstance.CallCount);
    }

    [Fact]
    public void BindServicesByAttribute_ShouldBindAttributeProperties()
    {
        IServiceProvider serviceProvider = GenerateServiceProvider();
        var propertyInstance = new TestRuntimePropertyService();

        DependencyBinder.BindServicesByAttribute<TestRuntimeAttribute>(serviceProvider, propertyInstance);

        propertyInstance.RunMethod();

        Assert.True(propertyInstance.WasCalled);
        Assert.Equal(1, propertyInstance.CallCount);
    }

    [Fact]
    public void BindServicesByAttribute_ShouldThrowIfServiceProviderIsNull() =>
        Assert.Throws<ArgumentNullException>(() =>
            DependencyBinder.BindServicesByAttribute<TestRuntimeAttribute>(null, new TestRuntimePropertyService()));

    private static ServiceProvider GenerateServiceProvider() => new ServiceCollection()
            .AddTransient<ITestRuntimeService, TestRuntimeFieldService>()
            .AddTransient<ITestRuntimeInternalServiceOne, TestRuntimeInternalServiceOne>()
            .AddTransient<ITestRuntimeInternalServiceTwo, TestRuntimeInternalServiceTwo>()
            .BuildServiceProvider();
}
