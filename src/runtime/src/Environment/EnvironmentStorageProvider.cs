// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime.Environment;

internal class EnvironmentStorageProvider : IEnvironmentStorageProvider
{
    private readonly ISessionState sessionState;

    public EnvironmentStorageProvider(ISessionState sessionState)
    {
        this.sessionState = sessionState;
    }

    public T GetEnvironmentVariable<T>(string key) => sessionState.GetValue<T>(key);

    public void SetEnvironmentVariable<T>(string key, T value) => sessionState.SetValue<T>(key, value);
}
