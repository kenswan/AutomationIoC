﻿// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.PowerShell.Tools.Test.TestBed.Services;

public interface ITestService
{
    int CallCount { get; }
    string CallTestMethod();
}

public class TestService : ITestService
{
    public int CallCount { get; private set; }

    public string CallTestMethod()
    {
        CallCount += 1;

        return "This is a message Test";
    }
}

public interface ITestServiceForProperty
{
    int CallCount { get; }
    string CallTestMethod();
}

public class TestServiceForProperty : TestService, ITestServiceForProperty
{ }
