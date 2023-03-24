// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Moq;
using Xunit;

namespace AutomationIoC.Runtime.Environment;

public class EnvironmentStorageProviderTests
{
    private readonly Mock<ISessionState> sessionStateMock;

    private readonly IEnvironmentStorageProvider environmentStorageProvider;

    public EnvironmentStorageProviderTests()
    {
        sessionStateMock = new();

        environmentStorageProvider = new EnvironmentStorageProvider(sessionStateMock.Object);
    }

    [Fact]
    public void ShouldGetEnvironmentVariable()
    {
        var key = Guid.NewGuid().ToString();
        var expectedValue = Guid.NewGuid();

        sessionStateMock.Setup(state => state.GetValue<Guid>(key)).Returns(expectedValue);

        Guid actualValue = environmentStorageProvider.GetEnvironmentVariable<Guid>(key);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ShouldReturnNullIfVariableDoesNotExist()
    {
        var key = Guid.NewGuid().ToString();
        string expectedValue = null;

        sessionStateMock.Setup(state => state.GetValue<string>(key)).Returns(expectedValue);

        var actualValue = environmentStorageProvider.GetEnvironmentVariable<string>(key);

        Assert.Null(actualValue);
    }

    [Fact]
    public void ShouldStoreEnvironmentVariable()
    {
        var key = Guid.NewGuid().ToString();
        var value = Guid.NewGuid();

        sessionStateMock.Setup(state => state.SetValue(key, value));

        var storageProvider = new EnvironmentStorageProvider(sessionStateMock.Object);

        storageProvider.SetEnvironmentVariable(key, value);

        sessionStateMock.Verify(state => state.SetValue(key, value), Times.Once);
    }
}
