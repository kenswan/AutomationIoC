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

            using var context = AutomationSandbox.CreateCommand<SetEnvironment>();

            context.ConfigureParameters(command =>
            {
                command
                    .AddParameter("Key", environmentKey)
                    .AddParameter("Value", expectedValue);
            });
           
            _ = context.RunCommand();

            context.ConfigureParameters(command =>
            {
                command.Clear();

                command.AddCommand("Get-Variable")
                    .AddParameter("Name", environmentKey);
            });

            var results = context.RunCommand();

            var result = results.FirstOrDefault().BaseObject as PSVariable;

            Assert.Single(results);
            Assert.Equal(expectedValue, result.Value);
        }
    }
}
