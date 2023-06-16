[![Nuget Version](https://img.shields.io/nuget/v/AutomationIoC?logo=nuget)](https://www.nuget.org/packages/AutomationIoC)
[![Nuget Downloads](https://img.shields.io/nuget/dt/AutomationIoC?logo=nuget)](https://www.nuget.org/packages/AutomationIoC)
[![Continuous Integration](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml)

# AutomationIoC.PSCmdlets

Dependency Injection Framework for C# PowerShell Cmdlets

## Requirements

- [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) or [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [PowerShell 7.3](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3) or greater

## Getting Started

### Installation

```powershell
dotnet add package AutomationIoC.PSCmdlets
```

### Development

_See [Sample Projects](https://github.com/kenswan/AutomationIoC/tree/main/sample) for a full sample solution_

Add the following to your .csproj file

```xml
<PropertyGroup>
    <!-- ... -->
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
</PropertyGroup>
```

Add a Startup class

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

Add a PowerShell Command with Dependencies Injected

Synchronous process example

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

Asynchronous process example

```csharp
[Cmdlet(VerbsLifecycle.Submit, "DataAsync")]
public class SubmitDataAsync : IoCShellAsync<Startup>
{
    [Parameter(Mandatory = true)]
    public string Id { get; set; }

    [AutomationDependency]
    protected ITestDependencyOne TestDependencyOne { get; set; }

    [AutomationDependency]
    protected ITestDependencyTwo testDependencyTwo { get; set; }

    [AutomationDependency]
    private ILogger<RequestCard> logger { get; set; }

    protected override async Task ProcessRecordAsync()
    {
        await TestDependencyOne.PushTestIdAsync(Id);

        TestData testData = await testDependencyTwo.GetTestDataAsync(Id);

        logger.LogInformation("Auto Generated Data: {Id} - {Name}", Id, testData.Name);

        WriteObject(testData);
    }
}
```

### Execution

In PowerShell terminal run the following:

```powershell
Import-Module <path-to-your-dll>/<your-assembly>.dll -V
```

You should see your custom commands listed in the verbose output (signaled by `-v`).

Now you are ready to run your own custom commands!

The following example is based on command name from above:

```powershell
Submit-Data -id "ThisIsATestId"
```

_See [Project - launch.json](https://github.com/kenswan/AutomationIoC/blob/main/sample/.vscode/launch.json) for a sample
on launching your module through VS Code_

## Resources

- [Microsoft Docs: Debug Compiled Cmdlets](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/vscode/using-vscode-for-debugging-compiled-cmdlets?view=powershell-7.2)
- [Microsoft Docs: VS Code PowerShell Development](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/vscode/using-vscode?view=powershell-7.2)

## Related Packages

| Package                                                                                       | Description                                                                     |
| --------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| [AutomationIoC.PSCmdlets.Tools](https://www.nuget.org/packages/AutomationIoC.PSCmdlets.Tools) | Tooling for running/testing C# PowerShell Cmdlets                               |
| [AutomationIoC](https://www.nuget.org/packages/AutomationIoC)                                 | Framework Runtime for AutomationIoC Console Applications and PowerShell Cmdlets |
| [AutomationIoC.Consoles](https://www.nuget.org/packages/AutomationIoC.Consoles)               | Dependency Injection Framework for C# Console Applications                      |
