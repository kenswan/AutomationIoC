// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime;

/// <summary>
/// Binds registered services to context object leveraging dependency injection
/// </summary>
public interface IAutomationBinder
{
    /// <summary>
    /// Binds registered services to context object leveraging dependency injection
    /// </summary>
    /// <typeparam name="TAttribute">Attribute of service that should receive injection</typeparam>
    /// <param name="instance">Instance of object that needs to have services injected</param>
    void BindContext<TAttribute>(object instance) where TAttribute : Attribute;
}
