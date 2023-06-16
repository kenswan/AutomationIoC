[![Nuget Version](https://img.shields.io/nuget/vpre/AutomationIoC?logo=nuget)](https://www.nuget.org/packages/AutomationIoC)
[![Nuget Downloads](https://img.shields.io/nuget/dt/AutomationIoC?logo=nuget)](https://www.nuget.org/packages/AutomationIoC)
[![Continuous Integration](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml)

# AutomationIoC.PSCmdlets.Tools

Tooling for running/testing C# PowerShell Cmdlets

## Requirements

- [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) or [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [PowerShell 7.3](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3) or greater

## Getting Started

### Installation

```powershell
dotnet add package AutomationIoC.PSCmdlets.Tools
```

### Testing AutomationIoC PSCmdlets

Sample AutomationIoC PSCmdlet startup class

```csharp
public class Startup : IoCStartup
{
    // Add any configuration sources or data needed
    public override void Configure(IConfigurationBuilder configurationBuilder)
    {
        // AutomationEnvironment class allows retrieving global PowerShell
        // variables during service configuration.
        var environment =
            AutomationEnvironment.GetVariable<string>("AppEnvironment");

        var appSettings = new Dictionary<string, string>()
        {
            ["TestOptionsConfig:TestProperty"] = "This is an example",
            ["AppEnvironment"] = environment
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    // Configure services/lifetimes
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<TestOptions>(Configuration.GetSection("TestOptionsConfig"));

        services.AddTransient<ITestDependencyOne, TestDependencyOne>();
        services.AddTransient<ITestDependencyTwo, TestDependencyTwo>();
        // ...
        // other services, client, etc.
    }
}
```

Sample AutomationIoC PSCmdlet to test

```csharp
[Cmdlet(VerbsLifecycle.Submit, "Data")]
public class SubmitData : IoCShell<Startup>
{
    [Parameter(Mandatory = true)]
    public string Id { get; set; }

    [AutomationDependency]
    protected ITestDependencyOne TestDependencyOne { get; set; }

    [AutomationDependency]
    protected ITestDependencyTwo testDependencyTwo { get; set; }

    [AutomationDependency]
    private ILogger<RequestCard> logger { get; set; }

    protected override void ProcessRecord()
    {
        base.ProcessRecord();

        TestDependencyOne.PushTestId(Id);

        TestData testData = testDependencyTwo.GetTestData(Id);

        logger.LogInformation("Auto Generated Data: {Id} - {Name}", Id, testData.Name);

        WriteObject(testData);
    }
}
```

Sample Test

```csharp
using AutomationIoC.PSCmdlets.Tools;
using FluentAssertions;
using Xunit;

public class RequestCardTests
{
    [Fact]
    public void ShouldSubmitData()
    {
        var testDependencyOneMock = new Mock<ITestDependencyOne>();
        var testDependencyTwoMock = new Mock<ITestDependencyTwo>();
        var testId = "ThisIsATestId";
        var expectedTestData = new TestData { Id = testId, Name = "ThisIsTestName" };

        testDependencyTwoMock.Setup(dependency =>
            dependency.GetTestDataAsync(testId))
                .Returns(expectedTestData);

        // Creates inline context of the command under test
        using var submitDataCommand =
            AutomationSandbox.CreateContext<SubmitData, Startup>(services =>
            {
                services.AddTransient(_ => testDependencyOneMock.Object);
                services.AddTransient(_ => testDependencyTwoMock.Object);
            });

        submitDataCommand.AddParameter(command =>
            command.AddParameter("id", testId));

        var actualTestData = submitDataCommand.RunCommand<TestData>().First();

        actualTestData.Should().BeEquivalentTo(expectedTestData);
    }
}
```

## Other Resources

- [Microsoft Docs: Debug Compiled Cmdlets](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/vscode/using-vscode-for-debugging-compiled-cmdlets?view=powershell-7.2)
- [Microsoft Docs: VS Code PowerShell Development](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/vscode/using-vscode?view=powershell-7.2)

## Related Packages

| Package                                                                           | Description                                                                     |
| --------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| [AutomationIoC.PSCmdlets](https://www.nuget.org/packages/AutomationIoC.PSCmdlets) | Dependency Injection Framework for C# PowerShell Cmdlets                        |
| [AutomationIoC](https://www.nuget.org/packages/AutomationIoC)                     | Framework Runtime for AutomationIoC Console Applications and PowerShell Cmdlets |
| [AutomationIoC.Consoles](https://www.nuget.org/packages/AutomationIoC.Consoles)   | Dependency Injection Framework for C# Console Applications                      |
