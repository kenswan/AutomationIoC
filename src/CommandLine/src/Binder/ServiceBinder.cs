// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.CommandLine.Binder;

internal static class ServiceBinder<T>
{
    public static T GetBoundValue(IServiceBinderActivator serviceBinderActivator)
    {
        IServiceProvider serviceProvider = serviceBinderActivator.GetServiceProvider();

        return serviceProvider.GetRequiredService<T>();
    }
}
