using Moq;
using System.Management.Automation;
using Xunit;

namespace PowerShellFocused.Test
{
    public class FocusedStartupTests
    {
        [Fact]
        public void ShouldExecute()
        {
            var commandRuntimeMock = new Mock<ICommandRuntime>();
            
            var startup = new TestStartup();
            startup.CommandRuntime = commandRuntimeMock.Object;
            
            var cmdlet = new TestCmdlet();
            cmdlet.CommandRuntime = commandRuntimeMock.Object;

            var task = Record.Exception(() =>
            {
                startup.RunInstance();
                cmdlet.RunInstance();
            });

            Assert.Null(task);
        }
    }
}