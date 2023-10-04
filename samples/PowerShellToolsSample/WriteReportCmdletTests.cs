// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Automation.PowerShell.Tools;
using PowerShellSample;
using System.Management.Automation;

namespace PowerShellToolsSample;

public class WriteReportCmdletTests
{
    [Fact]
    public void RunCommand_ShouldGenerateReportDetails()
    {
        using IAutomationCommand<WriteReportCmdlet> context =
            AutomationSandbox.CreateContext<WriteReportCmdlet, Startup>((services) =>
            { });

        ICollection<PSObject> results = context.RunCommand();

        Assert.Equal(3, results.Count);
    }
}
