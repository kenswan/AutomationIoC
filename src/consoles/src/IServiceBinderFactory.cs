// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine.Binding;

namespace AutomationIoC.Consoles;

public interface IServiceBinderFactory
{
    public BinderBase<T> Create<T>();
}
