namespace AutomationIoC.Integration.Services;

public class TestSdkFieldService : TestSdkService
{
    [AutomationDependency]
    protected ITestSdkInternalServiceOne testRuntimeInternalServiceOne = default;

    [AutomationDependency]
    protected ITestSdkInternalServiceTwo testRuntimeInternalServiceTwo = default;

    public override void RunMethod()
    {
        testRuntimeInternalServiceOne.RunMethod();
        testRuntimeInternalServiceTwo.RunMethod();

        base.RunMethod();
    }
}

public class TestSdkPropertyService : TestSdkService
{
    [AutomationDependency]
    protected ITestSdkInternalServiceOne TestSdkInternalServiceOne { get; set; }

    [AutomationDependency]
    protected ITestSdkInternalServiceTwo TestSdkInternalServiceTwo { get; set; }

    public override void RunMethod()
    {
        TestSdkInternalServiceOne.RunMethod();
        TestSdkInternalServiceTwo.RunMethod();

        base.RunMethod();
    }
}

public class TestSdkService : ITestSdkService
{
    public int CallCount { get; protected set; }

    public bool WasCalled { get; protected set; }

    public virtual void RunMethod()
    {
        WasCalled = true;
        CallCount += 1;
    }
}

public interface ITestSdkService
{
    int CallCount { get; }
    bool WasCalled { get; }

    void RunMethod();
}

public interface ITestSdkInternalServiceOne : ITestSdkService { }

public interface ITestSdkInternalServiceTwo : ITestSdkService { }
