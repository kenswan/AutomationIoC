namespace AutomationIoC.Runtime.Models
{
    internal class TestRuntimeFieldService : TestRuntimeService
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

    internal class TestRuntimePropertyService : TestRuntimeService
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

    internal class TestRuntimeService : ITestRuntimeService
    {
        public int CallCount { get; protected set; } = 0;

        public bool WasCalled { get; protected set; } = false;

        public virtual void RunMethod()
        {
            WasCalled = true;
            CallCount += 1;
        }
    }

    internal interface ITestRuntimeService
    {
        int CallCount { get; }
        bool WasCalled { get; }

        void RunMethod();
    }

    internal interface ITestRuntimeInternalServiceOne : ITestRuntimeService { }

    internal class TestRuntimeInternalServiceOne : TestRuntimeService, ITestRuntimeInternalServiceOne { }

    internal interface ITestRuntimeInternalServiceTwo : ITestRuntimeService { }

    internal class TestRuntimeInternalServiceTwo : TestRuntimeService, ITestRuntimeInternalServiceTwo { }
}
