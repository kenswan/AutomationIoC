﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Attributes;
using AutomationIoC.Runtime.Context;
using AutomationIoC.Runtime.Models;
using Moq;
using Xunit;

namespace AutomationIoC.Runtime.Binder;

public class AutomationIoCBinderTests
{
    private readonly Mock<IContextBuilder> contextBuilderMock;

    private readonly AutomationIoCBinder binder;

    public AutomationIoCBinderTests()
    {
        contextBuilderMock = new();

        binder = new(contextBuilderMock.Object);
    }

    [Fact]
    public void ShouldInitializeContext()
    {
        var instance = new TestInstance(123);

        contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(false);

        binder.BindContext<TestRuntimeAttribute>(instance);

        contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Once);

        contextBuilderMock.Verify(builder =>
            builder.InitializeCurrentInstance<TestRuntimeAttribute>(instance),
                Times.Once);
    }

    [Fact]
    public void ShouldNotInitializeContextIfAlreadySet()
    {
        var instance = new TestInstance(2);

        contextBuilderMock.Setup(builder => builder.IsInitialized).Returns(true);

        binder.BindContext<TestRuntimeAttribute>(instance);

        contextBuilderMock.Verify(builder => builder.BuildServices(), Times.Never);

        contextBuilderMock.Verify(builder =>
            builder.InitializeCurrentInstance<TestRuntimeAttribute>(instance),
                Times.Once);
    }
}
