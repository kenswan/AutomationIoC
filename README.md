[![Nuget Version](https://img.shields.io/nuget/v/AutomationIoC?logo=nuget)](https://www.nuget.org/packages/AutomationIoC)
[![Nuget Downloads](https://img.shields.io/nuget/dt/AutomationIoC?logo=nuget)](https://www.nuget.org/packages/AutomationIoC)
[![Continuous Integration](https://github.com/kenswan/BlazorFocused/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml)

# AutomationIoC

Dependency Injection Framework for C# PowerShell Cmdlets

## Requirements

- [Visual Studio Code](https://code.visualstudio.com/download)
- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [PowerShell 7](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2)

## Getting Started

### Installation

```powershell
dotnet add package AutomationIoC
```

### Development

_See [Sample Project](https://github.com/kenswan/AutomationIoC/tree/main/sample) for a full sample solution_

Add the following to your .csproj file

```xml
<PropertyGroup>
    <!-- ... -->
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
</PropertyGroup>
```

Add a PowerShell Startup Command

```csharp
[Cmdlet(VerbsLifecycle.Install, "Dependencies")]
public class Startup : AutomationStartup
{
    // Add any configuration sources or data needed
    public override void Configure(IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["TestOptionsConfig:TestProperty"] = "This is an example",
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    // Configure services/lifetimes
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<TestOptions>(Configuration.GetSection("TestOptionsConfig"));

        services.AddSingleton<TestService>();

        // other services, client, etc.
    }
}
```

Add a PowerShell Command with Dependencies Injected

Synchronous process example

```csharp
[Cmdlet(VerbsLifecycle.Submit, "TestData")]
public class SubmitTestData : IoCShell
{
    // Supports non-public properties
    [AutomationDependency]
    protected TestDependencyOne TestDependencyOne { get; set; }

    // Supports non-public fields
    [AutomationDependency]
    private readonly TestDependencyTwo testDependencyTwo;

    [AutomationDependency]
    private readonly ILogger<RequestCard> logger;

    protected override void ExecuteCmdlet()
    {
        string id = Guid.NewGuid();

        TestDependencyOne.PushTestId(id);

        TestData testData = testDependencyTwo.GetTestData(id);

        logger.LogInformation("Auto Generated Data: {Id} - {Name}", id, testData.Name);
    }
}
```

Asynchronous process example

```csharp
[Cmdlet(VerbsLifecycle.Submit, "TestDataAsync")]
public class SubmitTestDataAsync : IoCShellAsync
{
    // Supports non-public properties
    [AutomationDependency]
    protected TestDependencyOne TestDependencyOne { get; set; }

    // Supports non-public fields
    [AutomationDependency]
    private readonly TestDependencyTwo testDependencyTwo;

    [AutomationDependency]
    private readonly ILogger<RequestCard> logger;

    protected override async Task ExecuteCmdletAsync()
    {
        string id = Guid.NewGuid();

        await TestDependencyOne.PushTestIdAsync(id);

        TestData testData = await testDependencyTwo.GetTestDataAsync(id);

        logger.LogInformation("Auto Generated Data: {Id} - {Name}", id, testData.Name);
    }
}
```

### Execution

In PowerShell terminal run the following:

```powershell
Import-Module <path-to-your-dll>/<your-assembly>.dll -V
```

You should see your custom commands listed in the verbose output (signaled by `-v`)

_See [Project - launch.json](https://github.com/kenswan/AutomationIoC/blob/main/.vscode/launch.json) for a sample
on launching your module through VS Code_

Load the dependency injection (this will be the command you created for startup). _The following example is based on command name from above startup example_

```powershell
Load-Dependencies
```

Now you are ready to run your own custom commands!

### Testing

_NOTE: If using a constructor to inject dependencies for testing purposes, be sure to add an empty default
constructor as well_

Test Example Coming Soon

## Other Resources

- [Microsoft Docs: Debug Compiled Cmdlets](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/vscode/using-vscode-for-debugging-compiled-cmdlets?view=powershell-7.2)
- [Microsoft Docs: VS Code PowerShell Development](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/vscode/using-vscode?view=powershell-7.2)
