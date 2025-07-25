// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.PowerShell;

/// <summary>
/// Attribute used to annotate service dependency injection on corresponding services
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AutomationDependencyAttribute : Attribute
{
}
