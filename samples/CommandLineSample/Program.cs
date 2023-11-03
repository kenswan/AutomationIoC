// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.CommandLine;
using CommandLineSample;

IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("Sample CommandLine Example")
                .AddCommand<ReportCommand>("report");

IAutomationConsole console = builder.Build();

console.Run();
