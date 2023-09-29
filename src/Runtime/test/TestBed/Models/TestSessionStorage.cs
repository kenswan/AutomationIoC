// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Automation.Runtime.Test.TestBed.Models;

internal class TestSessionStorage : ISessionStorage
{
    public T GetValue<T>(string key) => throw new NotImplementedException();

    public void SetValue<T>(string key, T item) => throw new NotImplementedException();
}
