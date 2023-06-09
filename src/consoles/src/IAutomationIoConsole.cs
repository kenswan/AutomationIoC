// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace AutomationIoC.Consoles;

public interface IAutomationIoConsole
{
    int Run();

    Task<int> RunAsync();
}
