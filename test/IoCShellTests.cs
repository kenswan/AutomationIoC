using Microsoft.Extensions.DependencyInjection;
using Moq;
using AutomationIoC.Cmdlets;
using AutomationIoC.Services;
using System.Management.Automation;
using Xunit;

namespace AutomationIoC
{
    public class IoCShellTests
    {
        [Fact]
        public void ShouldCallExecuteMethod()
        {
            var commandRuntimeMock = new Mock<ICommandRuntime>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<TestService>();
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var psCmdlet = new TestShell(serviceProvider)
            {
                CommandRuntime = commandRuntimeMock.Object
            };

            psCmdlet.RunInstance();

            var testService = serviceProvider.GetRequiredService<TestService>();

            Assert.Equal(1, testService.CallCount);
        }
    }
}
