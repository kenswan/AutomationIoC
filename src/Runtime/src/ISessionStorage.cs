// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime;

/// <summary>
/// Environment storage used to store services for use in downstream automation
/// </summary>
public interface ISessionStorage
{
    /// <summary>
    /// Retrieves an object under a unique identifier
    /// </summary>
    /// <typeparam name="T">Type of object being retrieved</typeparam>
    /// <param name="key">Unique identifier address of stored object being retrieved</param>
    /// <returns>Current instance stored under unique identifier</returns>
    /// <remarks>Returns null if not established/stored</remarks>
    T GetValue<T>(string key);

    /// <summary>
    /// Stores an object under a unique identifier
    /// </summary>
    /// <typeparam name="T">Type of object being stored</typeparam>
    /// <param name="key">Unique identifier to use as address of stored object</param>
    /// <param name="item">Current instance to store under unique identifier</param>
    void SetValue<T>(string key, T item);
}
