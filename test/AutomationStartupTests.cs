using Microsoft.Extensions.DependencyInjection;
using Moq;
using AutomationIoC.Cmdlets;
using AutomationIoC.Services;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC
{
    public class AutomationStartupTests
    {
        [Fact]
        public void ShouldExecuteStartupMethods()
        {
            var commandRuntimeMock = new Mock<ICommandRuntime>();

            var automationStartup = new TestStartup
            {
                CommandRuntime = commandRuntimeMock.Object
            };

            automationStartup.RunInstance();


            Assert.Equal(2, automationStartup.CallCount);
        }
    }
}