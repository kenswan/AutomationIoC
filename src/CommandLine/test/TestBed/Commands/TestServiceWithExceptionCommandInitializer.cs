// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.CommandLine;

namespace AutomationIoC.CommandLine.Test.TestBed.Commands;

internal class TestServiceWithExceptionCommandInitializer : IAutomationCommandInitializer
{
    public void Initialize(IAutomationCommand command)
    {
        Option<string> passedInOption = new(name: "--test")
        {
            Description = "Description of test option field."
        };

        command.Add(passedInOption);

        command.SetAction((parseResult, automationContext) => throw new NotImplementedException());
    }
}
