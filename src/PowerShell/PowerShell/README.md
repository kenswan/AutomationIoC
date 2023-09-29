[![Nuget Version](https://img.shields.io/nuget/v/BlazorFocused.Automation.PowerShell?logo=nuget)](https://www.nuget.org/packages/BlazorFocused.Automation.PowerShell)
[![Nuget Downloads](https://img.shields.io/nuget/dt/BlazorFocused.Automation.PowerShell?logo=nuget)](https://www.nuget.org/packages/BlazorFocused.Automation.PowerShell)
[![Continuous Integration](https://github.com/BlazorFocused/Automation/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/BlazorFocused/Automation/actions/workflows/continuous-integration.yml)

# BlazorFocused.Automation.PowerShell

PowerShell PSCmdlet SDK utilities for Automation framework development

## NuGet Packages

| Package                                                                                                    | Description                                                            |
| ---------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------- |
| [BlazorFocused.Automation.PowerShell](https://www.nuget.org/packages/BlazorFocused.Automation.PowerShell/) | PowerShell PSCmdlet SDK utilities for Automation framework development |

## Documentation

Please visit the [BlazorFocused.Automation Documentation Site](https://BlazorFocused.github.io/Automation/) for installation, getting started, and API documentation.

## Samples

Please visit and/or download our [BlazorFocused.Automation.PowerShell Sample Solution](https://github.com/BlazorFocused/Automation/tree/main/samples/PowerShellSample) to get a more in-depth view of usage.

## Requirements

- [PowerShell 7.3](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3) or greater

## Installation

.NET CLI

```dotnetcli

dotnet add package BlazorFocused.Automation.PowerShell

```

### Development

Add the following to your .csproj file

```xml
<PropertyGroup>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
</PropertyGroup>
```

Add a Startup class

```csharp
public class Startup : AutomationStartup
{
    // Add any configuration sources or data needed
    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
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
    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
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
public class SubmitData : AutomationShell<Startup>
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
public class SubmitDataAsync : AutomationShellAsync<Startup>
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

- ## Related Packages

| Package                                                                                                                | Description                                                                     |
| ---------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| [BlazorFocused.Automation.Runtime](https://www.nuget.org/packages/BlazorFocused.Automation.Runtime/)                   | Runtime services for BlazorFocused.Automation framework                         |
| [BlazorFocused.Automation.CommandLine](https://www.nuget.org/packages/BlazorFocused.Automation.CommandLine/)           | Dependency Injection Framework for C# Console Applications                      |
| [BlazorFocused.Automation.PowerShell.Tools](https://www.nuget.org/packages/BlazorFocused.Automation.PowerShell.Tools/) | Development tools for running/testing PSCmdlets built with Automation framework |
