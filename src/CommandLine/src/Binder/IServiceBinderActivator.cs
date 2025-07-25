// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.CommandLine.Binder;

internal interface IServiceBinderActivator
{
    IServiceProvider GetServiceProvider();
}
