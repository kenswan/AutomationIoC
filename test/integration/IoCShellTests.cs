using AutomationIoC.Commands;
using AutomationIoC.Tools;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace AutomationIoC
{
    public class IoCShellTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public IoCShellTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldAddDependencies()
        {
            var expectedCount = 3;
            using var context = AutomationSandbox.CreateContext<TestModule, TestStartup>();

            var results = context.RunCommand();
            var result = results.First().BaseObject;

            var serializedResult = JsonSerializer.Serialize(result);
            testOutputHelper.WriteLine($"Call Count: {serializedResult}");

            var actualCount = Convert.ToInt32(result);

            Assert.Equal(expectedCount, actualCount);
        }
    }
}
