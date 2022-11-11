﻿using AutomationIoC.Integration.Attributes;

namespace AutomationIoC.Integration.Services;

public class TestRuntimeFieldService : TestRuntimeService
{
    [TestRuntime]
    protected ITestRuntimeInternalServiceOne testRuntimeInternalServiceOne = default;

    [TestRuntime]
    protected ITestRuntimeInternalServiceTwo testRuntimeInternalServiceTwo = default;

    public override void RunMethod()
    {
        testRuntimeInternalServiceOne.RunMethod();
        testRuntimeInternalServiceTwo.RunMethod();

        base.RunMethod();
    }
}

public class TestRuntimePropertyService : TestRuntimeService
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

public class TestRuntimeService : ITestRuntimeService
{
    public int CallCount { get; protected set; }

    public bool WasCalled { get; protected set; }

    public virtual void RunMethod()
    {
        WasCalled = true;
        CallCount += 1;
    }
}

public interface ITestRuntimeService
{
    int CallCount { get; }
    bool WasCalled { get; }

    void RunMethod();
}

public interface ITestRuntimeInternalServiceOne : ITestRuntimeService { }

public class TestRuntimeInternalServiceOne : TestRuntimeService, ITestRuntimeInternalServiceOne { }

public interface ITestRuntimeInternalServiceTwo : ITestRuntimeService { }

public class TestRuntimeInternalServiceTwo : TestRuntimeService, ITestRuntimeInternalServiceTwo { }
