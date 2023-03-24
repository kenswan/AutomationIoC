// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Management.Automation;

namespace AutomationIoC.Runtime.Environment;

internal interface IEnvironmentStorageProvider
{
    void SetEnvironmentVariable(string key, object value, ScopedItemOptions scopedItemOptions);

    T GetEnvironmentVariable<T>(string key);
}
