// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Automation.Runtime;

/// <summary>
/// Binds registered services to context object leveraging dependency injection
/// </summary>
public interface IContextBuilder
{
    /// <summary>
    /// Detects if a context service provider has been initialized/stored
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Builds a service provider for use in downstream automation
    /// </summary>
    /// <returns>Service Provider built in context of provided <see cref="IAutomationStartup"/></returns>
    /// <remarks></remarks>
    IServiceProvider BuildServices();

    /// <summary>
    /// Builds a service provider for use in downstream automation
    /// </summary>
    /// <param name="servicesOverride">Override preset services to append runtime services and store service provider</param>
    /// <returns>Service Provider built in context of provided <see cref="IAutomationStartup"/></returns>
    IServiceProvider BuildServices(Action<IServiceCollection> servicesOverride);

    /// <summary>
    /// Retrieves previously built/stored context service provider
    /// </summary>
    /// <returns>Service Provider built in context of provided <see cref="IAutomationStartup"/></returns>
    /// <remarks>
    /// This internally calls <see cref="BuildServices()" /> if a cached
    /// context service provider does not exist from an earlier build
    /// </remarks>
    IServiceProvider GetContextServiceProvider();
}
