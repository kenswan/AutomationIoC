// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Automation.Runtime.Test.TestBed.Models;

internal class TestSessionStorage : ISessionStorage
{
    private readonly Dictionary<string, object> storage;

    public TestSessionStorage()
    {
        storage = new Dictionary<string, object>();
    }

    public T GetValue<T>(string key)
    {
        bool hasValue = storage.TryGetValue(key, out object value);

        return hasValue ? (T)value : default;
    }

    public void SetValue<T>(string key, T item) => storage.TryAdd(key, item);
}
