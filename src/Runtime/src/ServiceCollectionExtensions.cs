// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime;

/// <summary>
/// Extension methods for service collection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Replaces a registered service with a new instance
    /// </summary>
    /// <typeparam name="TService">Type of service being replaced</typeparam>
    /// <param name="services">Service collection container of service being replaced</param>
    /// <param name="implementationFactory">Configured service replacement object</param>
    /// <returns>Updated Service Collection containing replaced instance/service</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public static IServiceCollection ReplaceRegisteredService<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
    {
        Type replacedServiceType = typeof(TService);

        // Remove previous service registration
        ServiceDescriptor serviceDescriptor =
            services.FirstOrDefault(descriptor => descriptor.ServiceType == replacedServiceType);

        if (serviceDescriptor is not null)
        {
            services.Remove(serviceDescriptor);
        }
        else
        {
            throw new InvalidOperationException($"Service of type {replacedServiceType} is not registered");
        }

        ServiceLifetime serviceLifetime = serviceDescriptor?.Lifetime ?? ServiceLifetime.Transient;

        IServiceCollection _ = serviceLifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(replacedServiceType, (serviceProvider) => implementationFactory(serviceProvider)),
            ServiceLifetime.Scoped => services.AddScoped(replacedServiceType, (serviceProvider) => implementationFactory(serviceProvider)),
            ServiceLifetime.Singleton => services.AddSingleton(replacedServiceType, (serviceProvider) => implementationFactory(serviceProvider)),
            _ => throw new NotImplementedException($"Lifetime {serviceLifetime} not supported for removal")
        };

        return services;
    }
}
