// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime;

public interface IAutomationEnvironment
{
    T GetVariable<T>(string key);

    bool TryGetVariable<T>(string key, out T value);
}
