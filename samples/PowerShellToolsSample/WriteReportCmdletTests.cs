// -------------------------------------------------------
// Copyright (c) Ken Swan. All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using AutomationSamples.Shared.Services;
using AutomationIoC.PowerShell.Tools;
using AutomationIoC.Runtime;
using Bogus;
using Moq;
using PowerShellSample;
using System.Management.Automation;

namespace PowerShellToolsSample;

public class WriteReportCmdletTests
{
    [Fact]
    public void RunCommand_ShouldGenerateReportDetails()
    {
        using IPowerShellAutomation<Startup> powerShellAutomation = AutomationSandbox.CreateSession<Startup>();

        ICollection<PSObject> results = powerShellAutomation.RunAutomationCommand<WriteReportCmdlet>();

        Assert.Equal(3, results.Count);
    }

    [Fact]
    public void RunCommand_ShouldPrint_Header_Data_Disclaimer()
    {
        string inputHeader = new Faker().Lorem.Sentence();
        string expectedHeader = new Faker().Lorem.Sentence();
        string expectedData = new Faker().Random.AlphaNumeric(20);
        string expectedDisclaimer = new Faker().Lorem.Sentence();

        var reportServiceMock = new Mock<IReportService>();

        reportServiceMock.Setup(service => service.GenerateReportHeader(inputHeader))
            .Returns(expectedHeader);

        reportServiceMock.Setup(service => service.GenerateReportData())
            .Returns(expectedData);

        reportServiceMock.Setup(service => service.GenerateReportDisclaimer())
            .Returns(expectedDisclaimer);

        using IPowerShellAutomation<Startup> powerShellAutomation =
            AutomationSandbox.CreateSession<Startup>(services =>
                // Replace the registered service with the mock
                services.ReplaceRegisteredService<IReportService>((_) => reportServiceMock.Object));

        ICollection<string> actualResults =
            powerShellAutomation.RunAutomationCommand<WriteReportCmdlet, string>(command =>
            {
                command.AddParameter("Header", inputHeader);
            });

        Assert.Equal(3, actualResults.Count);

        Assert.Collection(actualResults,
            actualHeader => Assert.Equal(expectedHeader, actualHeader),
            actualData => Assert.Equal(expectedData, actualData),
            actualDisclaimer => Assert.Equal(expectedDisclaimer, actualDisclaimer));
    }
}
