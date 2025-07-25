// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Runtime.Dependency;

namespace AutomationIoC.Runtime.Binder;

internal class AutomationBinder : IAutomationBinder
{
    private readonly IContextBuilder contextBuilder;

    public AutomationBinder(IContextBuilder contextBuilder)
    {
        this.contextBuilder = contextBuilder;
    }

    public void BindContext<TAttribute>(object instance)
        where TAttribute : Attribute
    {
        IServiceProvider serviceProvider =
            (!contextBuilder.IsInitialized) ?
                contextBuilder.BuildServices() :
                contextBuilder.GetContextServiceProvider();

        DependencyBinder.BindServicesByAttribute<TAttribute>(serviceProvider, instance);
    }
}
