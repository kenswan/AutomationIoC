// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;

namespace AutomationIoC.CommandLine.Test.TestBed.Services;

public class TestConfigurationService(IConfiguration configuration)
{
    public string GetConfigurationValue(string key) =>
        configuration.GetValue<string>(key) ?? throw new KeyNotFoundException($"Configuration key '{key}' not found.");

    public Task<string> GetConfigurationValueAsync(string key, CancellationToken cancellationToken = default) =>
        Task.FromResult(GetConfigurationValue(key));
}
