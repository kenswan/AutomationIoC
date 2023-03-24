// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime.Models;
internal class TestSessionState : ISessionState
{
    public T GetValue<T>(string key) => throw new NotImplementedException();

    public void SetValue<T>(string key, T item) => throw new NotImplementedException();
}
