using AutomationIoC.Runtime.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AutomationIoC.Runtime.Environment
{
    public class AutomationEnvironmentTests
    {
        private Mock<IEnvironmentStorageProvider> storageProviderMock;

        private IAutomationEnvironment automationEnvironment;

        public AutomationEnvironmentTests()
        {
            storageProviderMock = new Mock<IEnvironmentStorageProvider>();

            automationEnvironment = new AutomationEnvironment(storageProviderMock.Object);
        }

        [Fact]
        public void ShouldGetEnvironmentVariable()
        {
            var id = Guid.NewGuid();
            var key = id.ToString();
            var model = new TestModel(id);

            storageProviderMock.Setup(provider =>
                provider.GetEnvironmentVariable<TestModel>(key)).Returns(model);

            var actualModel = automationEnvironment.GetVariable<TestModel>(key);

            actualModel.Should().BeEquivalentTo(model);
        }

        [Fact]
        public void ShouldThrowIfVariableNotFound()
        {
            var key = Guid.NewGuid().ToString();

            storageProviderMock.Setup(provider =>
                provider.GetEnvironmentVariable<TestModel>(key)).Returns(null as TestModel);

            Assert.Throws<ArgumentException>(() => automationEnvironment.GetVariable<TestModel>(key));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShouldSendVaraibleIfExistReturnFalseIfNot(bool variableExists)
        {
            var id = Guid.NewGuid();
            var key = id.ToString();
            TestModel model = variableExists ? new TestModel(id) : null as TestModel;

            storageProviderMock.Setup(provider =>
                provider.GetEnvironmentVariable<TestModel>(key)).Returns(model);

            var actualVariableExists =
                automationEnvironment.TryGetVariable<TestModel>(key, out TestModel actualValue);

            actualValue.Should().BeEquivalentTo(model);

            Assert.Equal(variableExists, actualVariableExists);
        }
    }
}
