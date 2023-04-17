// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationIoC.Consoles.Builder;

namespace AutomationIoC.Consoles;

public static class AutomationIoConsole
{
    public static IAutomationIoConsoleBuilder CreateDefaultBuilder(string appName) =>
        new AutomationIoConsoleBuilder(appName);
}
