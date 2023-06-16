// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Consoles;
using ConsolesSample.Commands;

namespace ConsolesSample;

internal class Program
{
    private static int Main(string[] args)
    {
        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder(args)
                .AddCommand<ReportCommand>("report");

        IAutomationIoConsole console = builder.Build();

        return console.Run();
    }
}
