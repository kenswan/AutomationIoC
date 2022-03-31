using AutomationIoC.Integration.Commands;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC.Tools.Command
{
    public class AutomationCommandTests
    {
        [Fact]
        public void ShouldRunExternalCommands()
        {
            var environmentKey = Guid.NewGuid().ToString();
            var expectedValue = Guid.NewGuid().ToString();

            using var automationCommand = new AutomationCommand<TestCommand>();

            automationCommand.RunExternalCommand("Set-Variable", command =>
                command
                    .AddParameter("Name", environmentKey)
                    .AddParameter("Value", expectedValue));

            var results = automationCommand.RunExternalCommand("Get-Variable", command =>
                command.AddParameter("Name", environmentKey));

            var result = results.FirstOrDefault().BaseObject as PSVariable;

            Assert.Single(results);
            Assert.Equal(expectedValue, result.Value);
        }
    }
}
