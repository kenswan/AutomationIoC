using AutomationIoC.SDK;
using AutomationIoC.Tools;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC
{
    public class SetEnvironmentTests
    {
        [Fact]
        public void ShouldSetEnvironmentVariables()
        {
            var environmentKey = "TestKey";
            var expectedValue = Guid.NewGuid().ToString();

            using var command = AutomationSandbox.CreateCommand<SetEnvironment>();

            command.RunCommand(command =>
                command
                    .AddParameter("Key", environmentKey)
                    .AddParameter("Value", expectedValue));

            var results = command.RunExternalCommand<PSVariable>("Get-Variable", command =>
                command.AddParameter("Name", environmentKey));

            Assert.Single(results);
            Assert.Equal(expectedValue, results.First().Value);
        }
    }
}
