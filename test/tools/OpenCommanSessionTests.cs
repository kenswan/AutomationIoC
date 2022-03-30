using AutomationIoC.Tools.Command;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC.Tools
{
    public class OpenCommanSessionTests
    {
        private readonly IOpenCommandSession openCommandSession;
        public OpenCommanSessionTests()
        {
            openCommandSession = new OpenCommandSession();
        }

        [Fact]
        public void ShouldRunCommands()
        {
            var environmentKey = "TestKey";
            var expectedValue = Guid.NewGuid().ToString();

            _ = openCommandSession.RunCommand("Set-Variable", command =>
                command
                    .AddParameter("Name", environmentKey)
                    .AddParameter("Value", expectedValue));

            var results = openCommandSession.RunCommand("Get-Variable", command =>
                command.AddParameter("Name", environmentKey));

            var result = results.FirstOrDefault().BaseObject as PSVariable;

            Assert.Single(results);
            Assert.Equal(expectedValue, result.Value);
        }
    }
}
