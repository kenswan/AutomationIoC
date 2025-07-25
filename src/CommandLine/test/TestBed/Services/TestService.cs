// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Configuration;

namespace AutomationIoC.CommandLine.Test.TestBed.Services;

public interface ITestService
{
    public void Execute(string data);
}

public class TestService : ITestService
{
    public const string CONFIG_KEY = "TestConfiguration";
    public const string CONFIG_VALUE = "ExecutedConfiguration";

    public TestService(IConfiguration configuration)
    {
        string configurationValue = configuration.GetValue<string>(CONFIG_KEY);

        if (configurationValue != CONFIG_VALUE)
        {
            throw new TestServiceConfigurationException("Test Service is not configured properly");
        }
    }

    public void Execute(string data) => Console.WriteLine("Executed Data:{0}", data);
}

public class TestServiceConfigurationException(string message) : Exception(message);
