﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Runtime;

public class DependencyContext<TAttribute, TStartup>
    where TAttribute : Attribute
    where TStartup : IIoCStartup, new()
{
    public ISessionState SessionState { get; set; }

    public object Instance { get; set; }
}
