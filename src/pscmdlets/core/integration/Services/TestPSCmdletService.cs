// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.PSCmdlets.Integration.Services;

public class TestPSCmdletFieldService : TestPSCmdletService
{
    [AutomationDependency]
    protected ITestPSCmdletInternalServiceOne testRuntimeInternalServiceOne = default;

    [AutomationDependency]
    protected ITestPSCmdletInternalServiceTwo testRuntimeInternalServiceTwo = default;

    public override void RunMethod()
    {
        testRuntimeInternalServiceOne.RunMethod();
        testRuntimeInternalServiceTwo.RunMethod();

        base.RunMethod();
    }
}

public class TestPSCmdletPropertyService : TestPSCmdletService
{
    [AutomationDependency]
    protected ITestPSCmdletInternalServiceOne TestSdkInternalServiceOne { get; set; }

    [AutomationDependency]
    protected ITestPSCmdletInternalServiceTwo TestSdkInternalServiceTwo { get; set; }

    public override void RunMethod()
    {
        TestSdkInternalServiceOne.RunMethod();
        TestSdkInternalServiceTwo.RunMethod();

        base.RunMethod();
    }
}

public class TestPSCmdletService : ITestPSCmdletService
{
    public int CallCount { get; protected set; }

    public bool WasCalled { get; protected set; }

    public virtual void RunMethod()
    {
        WasCalled = true;
        CallCount += 1;
    }
}

public interface ITestPSCmdletService
{
    int CallCount { get; }
    bool WasCalled { get; }

    void RunMethod();
}

public interface ITestPSCmdletInternalServiceOne : ITestPSCmdletService { }

public interface ITestPSCmdletInternalServiceTwo : ITestPSCmdletService { }
