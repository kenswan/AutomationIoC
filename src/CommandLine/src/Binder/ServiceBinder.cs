// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Binding;

namespace AutomationIoC.CommandLine.Binder;

internal class ServiceBinder<T> : BinderBase<T>
{
    private readonly IServiceBinderActivator serviceBinderActivator;

    public ServiceBinder(IServiceBinderActivator serviceBinderActivator)
    {
        this.serviceBinderActivator = serviceBinderActivator;
    }

    protected override T GetBoundValue(BindingContext bindingContext)
    {
        IServiceProvider serviceProvider = serviceBinderActivator.GetServiceProvider();

        return serviceProvider.GetRequiredService<T>();
    }
}
