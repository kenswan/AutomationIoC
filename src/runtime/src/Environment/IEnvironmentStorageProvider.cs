// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime.Environment;

internal interface IEnvironmentStorageProvider
{
    void SetEnvironmentVariable<T>(string key, T value);

    T GetEnvironmentVariable<T>(string key);
}
