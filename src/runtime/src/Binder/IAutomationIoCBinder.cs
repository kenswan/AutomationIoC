// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime.Binder;

internal interface IAutomationIoCBinder
{
    void BindContext<TAttribute>(object instance) where TAttribute : Attribute;
}
