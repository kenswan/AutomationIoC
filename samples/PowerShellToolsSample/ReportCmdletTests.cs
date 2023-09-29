// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using PowerShellSample;
using System.Management.Automation;
using BlazorFocused.Automation.PowerShell.Tools;

namespace PowerShellToolsSample;

public class ReportCmdletTests
{
    [Fact]
    public void RunCommand_ShouldGenerateReportDetails()
    {
        using IAutomationCommand<ReportCmdlet> context =
            AutomationSandbox.CreateContext<ReportCmdlet, Startup>((services) =>
            { });

        ICollection<PSObject> results = context.RunCommand();

        Assert.Equal(3, results.Count);
    }
}
