using AutomationIoC.Runtime.Session;
using Moq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Xunit;

namespace AutomationIoC.Runtime.Environment;

public class EnvironmentStorageProviderTests
{
    [Fact]
    public void ShouldGetEnvironmentVariable()
    {
        var key = Guid.NewGuid().ToString();
        var expectedValue = Guid.NewGuid();
        var scope = ScopedItemOptions.Unspecified;

        var sessionStateMock = new Mock<ISessionState>();
        PSVariable serviceVariable = new(key, expectedValue, scope);

        var runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
        runspace.Open();

        var psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

        psVariableIntrinsics.Set(serviceVariable);

        sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);

        var storageProvider = new EnvironmentStorageProvider(sessionStateMock.Object);

        var actualValue = storageProvider.GetEnvironmentVariable<Guid>(key);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ShouldReturnNullIfVariableDoesNotExist()
    {
        var key = Guid.NewGuid().ToString();
        var value = Guid.NewGuid().ToString();

        var sessionStateMock = new Mock<ISessionState>();

        var runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
        runspace.Open();

        var psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

        sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);

        var storageProvider = new EnvironmentStorageProvider(sessionStateMock.Object);

        var actualValue = storageProvider.GetEnvironmentVariable<Guid?>(key);

        Assert.Null(actualValue);
    }

    [Fact]
    public void ShouldStoreEnvironmentVariable()
    {
        var key = Guid.NewGuid().ToString();
        var value = Guid.NewGuid();
        var scope = ScopedItemOptions.Constant;

        var sessionStateMock = new Mock<ISessionState>();

        PSVariable serviceVariable =
                new(key, value, scope);

        var runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
        runspace.Open();

        var psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

        psVariableIntrinsics.Set(serviceVariable);

        sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);
    }
}
