using Microsoft.Extensions.DependencyInjection;
using Moq;
using PowerShellFocused.Cmdlets;
using PowerShellFocused.Services;
using System.Management.Automation;
using Xunit;

namespace PowerShellFocused
{
    public class FocusedCmdletTests
    {
        [Fact]
        public void ShouldCallExecuteMethod()
        {
            var commandRuntimeMock = new Mock<ICommandRuntime>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<TestService>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var psCmdlet = new TestCmdlet(serviceProvider)
            {
                CommandRuntime = commandRuntimeMock.Object
            };

            psCmdlet.RunInstance();

            var testService = serviceProvider.GetRequiredService<TestService>();

            Assert.Equal(1, testService.CallCount);
        }
    }
}
