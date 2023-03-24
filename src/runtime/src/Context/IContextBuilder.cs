// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace AutomationIoC.Runtime.Context;

internal interface IContextBuilder
{
    bool IsInitialized { get; }

    void BuildServices();

    void BuildServices(IServiceCollection serviceCollection);

    void InitializeCurrentInstance<TAttribute>(object instance) where TAttribute : Attribute;
}
