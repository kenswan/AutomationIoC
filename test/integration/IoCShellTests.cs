using AutomationIoC.Commands;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
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
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ImportPSModule(new string[] { typeof(TestModule).Assembly.Location });
            
            Runspace runspace = RunspaceFactory.CreateRunspace(initial);
            runspace.Open();
            
            PowerShell ps = PowerShell.Create();
            ps.Runspace = runspace;
            ps.Commands.AddCommand("Get-Test");

            var result = ps.Invoke().First().BaseObject;
            var serializedResult = JsonSerializer.Serialize(result);
            testOutputHelper.WriteLine($"Call Count: {serializedResult}");

            var actualCount = Convert.ToInt32(result);
            
            Assert.Equal(expectedCount, actualCount);
        }
    }
}
