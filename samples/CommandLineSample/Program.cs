// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.CommandLine;

namespace CommandLineSample;

internal class Program
{
    public static void Main(string[] args)
    {
        IAutomationConsoleBuilder builder =
            AutomationConsole.CreateDefaultBuilder("Sample CommandLine Example", args)
                .AddCommand<ReportCommand>("report");

        IAutomationConsole console = builder.Build();

        console.Run();
    }
}

