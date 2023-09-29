// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Moq;
using BlazorFocused.Automation.Runtime.Binder;
using BlazorFocused.Automation.Runtime.Test.TestBed.Attributes;
using BlazorFocused.Automation.Runtime.Test.TestBed.Services;

namespace BlazorFocused.Automation.Runtime.Test.Binder;

public class AutomationBinderTests
{
    private readonly Mock<IContextBuilder> contextBuilderMock;
    private readonly Mock<ITestRuntimeInternalServiceOne> testBinderServiceOneMock;
    private readonly Mock<ITestRuntimeInternalServiceTwo> testBinderServiceTwoMock;

    private readonly AutomationBinder binder;

    public AutomationBinderTests()
    {
        contextBuilderMock = new();
        testBinderServiceOneMock = new();
        testBinderServiceTwoMock = new();

        binder = new(contextBuilderMock.Object);
    }

    [Fact]
    public void BindContext_ShouldInitializeContext()
    {
        // Arrange
        var instance = new TestAutomationBinderService();
        IServiceProvider serviceProvider = GenerateServiceProvider();

        contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(false);
        contextBuilderMock.Setup(builder => builder.BuildServices()).Returns(serviceProvider);

        // Act
        binder.BindContext<TestRuntimeAttribute>(instance);

        instance.RunMethod();

        // Assert
        contextBuilderMock.VerifyGet(builder => builder.IsInitialized, Times.Once);
        contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Once);

        testBinderServiceOneMock.Verify(service => service.RunMethod(), Times.Once);
        testBinderServiceTwoMock.Verify(service => service.RunMethod(), Times.Once);
    }

    [Fact]
    public void BindContext_ShouldNotInitializeContextIfAlreadySet()
    {
        // Arrange
        var instance = new TestAutomationBinderService();
        IServiceProvider serviceProvider = GenerateServiceProvider();

        contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(true);

        contextBuilderMock.Setup(builder =>
            builder.GetContextServiceProvider()).Returns(serviceProvider);

        // Act
        binder.BindContext<TestRuntimeAttribute>(instance);

        instance.RunMethod();

        // Assert
        contextBuilderMock.VerifyGet(builder => builder.IsInitialized, Times.Once);
        contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Never);

        testBinderServiceOneMock.Verify(service => service.RunMethod(), Times.Once);
        testBinderServiceTwoMock.Verify(service => service.RunMethod(), Times.Once);
    }

    public IServiceProvider GenerateServiceProvider() =>
        new ServiceCollection()
            .AddScoped<ITestRuntimeInternalServiceOne>(_ => testBinderServiceOneMock.Object)
            .AddScoped<ITestRuntimeInternalServiceTwo>(_ => testBinderServiceTwoMock.Object)
            .BuildServiceProvider();

    public class TestAutomationBinderService : TestRuntimeService, ITestRuntimeService
    {
        [TestRuntime]
        protected ITestRuntimeInternalServiceOne TestRuntimeInternalServiceOne { get; set; }

        [TestRuntime]
        protected ITestRuntimeInternalServiceTwo TestRuntimeInternalServiceTwo { get; set; }

        public override void RunMethod()
        {
            TestRuntimeInternalServiceOne.RunMethod();
            TestRuntimeInternalServiceTwo.RunMethod();

            base.RunMethod();
        }
    }
}
