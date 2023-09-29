// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using CommandLineSample;
using BlazorFocused.Automation.CommandLine;

IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("Sample CommandLine Example")
                .AddCommand<ReportCommand>("report");

IAutomationConsole console = builder.Build();

console.Run();
