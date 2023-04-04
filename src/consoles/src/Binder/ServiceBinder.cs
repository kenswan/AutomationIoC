// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Binding;

namespace AutomationIoC.Consoles.Binder;

internal class ServiceBinder<T> : BinderBase<T>
{
    private readonly IServiceProvider serviceProvider;

    public ServiceBinder(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override T GetBoundValue(BindingContext bindingContext) => serviceProvider.GetRequiredService<T>();
}
