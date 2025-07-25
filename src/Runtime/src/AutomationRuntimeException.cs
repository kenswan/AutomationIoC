// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime;

/// <summary>
///     Exception thrown when an error occurs in the automation runtime.
/// </summary>
/// <param name="message">Exception message</param>
public class AutomationRuntimeException(string message) : Exception(message);
