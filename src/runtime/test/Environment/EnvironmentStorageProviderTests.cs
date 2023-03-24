// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Session;
using Moq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Xunit;

namespace AutomationIoC.Runtime.Environment;

public class EnvironmentStorageProviderTests
{
    [Fact(Skip = "Moving PS Out of Runtime project for 2.0")]
    public void ShouldGetEnvironmentVariable()
    {
        var key = Guid.NewGuid().ToString();
        var expectedValue = Guid.NewGuid();
        ScopedItemOptions scope = ScopedItemOptions.Unspecified;

        var sessionStateMock = new Mock<ISessionState>();
        PSVariable serviceVariable = new(key, expectedValue, scope);

        Runspace runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
        runspace.Open();

        PSVariableIntrinsics psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

        psVariableIntrinsics.Set(serviceVariable);

        sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);

        var storageProvider = new EnvironmentStorageProvider(sessionStateMock.Object);

        Guid actualValue = storageProvider.GetEnvironmentVariable<Guid>(key);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact(Skip = "Moving PS Out of Runtime project for 2.0")]
    public void ShouldReturnNullIfVariableDoesNotExist()
    {
        var key = Guid.NewGuid().ToString();
        var value = Guid.NewGuid().ToString();

        var sessionStateMock = new Mock<ISessionState>();

        Runspace runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
        runspace.Open();

        PSVariableIntrinsics psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

        sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);

        var storageProvider = new EnvironmentStorageProvider(sessionStateMock.Object);

        Guid? actualValue = storageProvider.GetEnvironmentVariable<Guid?>(key);

        Assert.Null(actualValue);
    }

    [Fact(Skip = "Moving PS Out of Runtime project for 2.0")]
    public void ShouldStoreEnvironmentVariable()
    {
        var key = Guid.NewGuid().ToString();
        var value = Guid.NewGuid();
        ScopedItemOptions scope = ScopedItemOptions.Constant;

        var sessionStateMock = new Mock<ISessionState>();

        PSVariable serviceVariable =
                new(key, value, scope);

        Runspace runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault());
        runspace.Open();

        PSVariableIntrinsics psVariableIntrinsics = runspace.SessionStateProxy.PSVariable;

        psVariableIntrinsics.Set(serviceVariable);

        sessionStateMock.SetupGet(state => state.PSVariable).Returns(psVariableIntrinsics);
    }
}
