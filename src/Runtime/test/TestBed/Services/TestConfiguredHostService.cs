// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AutomationIoC.Runtime.Test.TestBed.Services;

public class TestConfiguredHostService(
    IOptions<TestConfiguredHostServiceOptions> options,
    IConfiguration configuration)
{
    public string FieldOne => options.Value.FieldOne;
    public string FieldTwo => options.Value.FieldTwo;

    public bool GetKeyValue(string key, out string? value)
    {
        value = configuration[key];
        return value is not null;
    }
}

public class TestConfiguredHostServiceOptions
{
    public string FieldOne { get; set; }
    public string FieldTwo { get; set; }
}
