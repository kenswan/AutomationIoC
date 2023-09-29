// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Immutable;

namespace BlazorFocused.Automation.Runtime;

/// <inheritdoc />
public abstract class AutomationStartup : IAutomationStartup
{
    /// <inheritdoc/>
    public virtual void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder) { }

    /// <inheritdoc/>
    public virtual void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) { }

    /// <inheritdoc/>
    public virtual IDictionary<string, string> GenerateParameterConfigurationMapping() =>
        ImmutableDictionary<string, string>.Empty;

    /// <inheritdoc/>
    public virtual string[] GenerateParameters() => Environment.GetCommandLineArgs();
}
