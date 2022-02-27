using Microsoft.Extensions.DependencyInjection;
using Moq;
using AutomationIoC.Cmdlets;
using AutomationIoC.Services;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC
{
    public class FocusedStartupTests
    {
        [Fact]
        public void ShouldExecuteStartupMethods()
        {
            var commandRuntimeMock = new Mock<ICommandRuntime>();

            var startup = new TestStartup
            {
                CommandRuntime = commandRuntimeMock.Object
            };

            startup.RunInstance();

            var serviceProvider = startup.InternalServiceProvider;
            var testService = serviceProvider.GetRequiredService<TestService>();

            Assert.Equal(2, testService.CallCount);
        }
    }
}