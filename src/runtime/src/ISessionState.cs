// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime;

public interface ISessionState
{
    T GetValue<T>(string key);

    void SetValue<T>(string key, T item);
}
