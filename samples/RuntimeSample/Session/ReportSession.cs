// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.Runtime;

namespace RuntimeSample.Session;

internal class ReportSession : ISessionStorage
{
    private readonly Dictionary<string, object> tempStorage = new();

    public T GetValue<T>(string key)
    {
        bool keyExists = tempStorage.TryGetValue(key, out object value);

        return (T)(keyExists ? value : default(T));
    }

    public void SetValue<T>(string key, T item) => tempStorage[key] = item;
}
