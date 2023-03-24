// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime.Dependency;

internal interface IDependencyBinder
{
    void LoadFieldsByAttribute<TAttribute>(object instance) where TAttribute : Attribute;

    void LoadPropertiesByAttribute<TAttribute>(object instance) where TAttribute : Attribute;
}
