[![Nuget Version](https://img.shields.io/nuget/v/BlazorFocused.Automation.CommandLine?logo=nuget)](https://www.nuget.org/packages/BlazorFocused.Automation.CommandLine)
[![Nuget Downloads](https://img.shields.io/nuget/dt/BlazorFocused.Automation.CommandLine?logo=nuget)](https://www.nuget.org/packages/BlazorFocused.Automation.CommandLine)
[![Continuous Integration](https://github.com/BlazorFocused/Automation/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/BlazorFocused/Automation/actions/workflows/continuous-integration.yml)

# BlazorFocused.Automation.Runtime

Dependency Injection Framework for C# Console Applications

## NuGet Packages

| Package                                                                                                      | Description                                                |
| ------------------------------------------------------------------------------------------------------------ | ---------------------------------------------------------- |
| [BlazorFocused.Automation.CommandLine](https://www.nuget.org/packages/BlazorFocused.Automation.CommandLine/) | Dependency Injection Framework for C# Console Applications |

## Documentation

Please visit the [BlazorFocused.Automation Documentation Site](https://BlazorFocused.github.io/Automation/) for installation, getting started, and API documentation.

## Samples

Please visit and/or download our [BlazorFocused.Automation.CommandLine Sample Solution](https://github.com/BlazorFocused/Automation/tree/main/samples/CommandLineSample) to get a more in-depth view of usage.

## Installation

.NET CLI

```dotnetcli

dotnet add package BlazorFocused.Automation.CommandLine

```

### Development

Create Automation Console Command to run

```csharp
public class TestCommand : StandardCommand
{
    public override void ConfigureCommand(IServiceBinderFactory serviceBinderFactory, Command command)
    {
        var greetingOption = new Option<string>(
            name: "--greeting",
            description: "New greeting to display.");

        command.AddOption(greetingOption);

        command.SetHandler(UpdateGreeting,
            greetingOption,
            serviceBinderFactory.Bind<ITestService>(),
            serviceBinderFactory.Bind<IConfiguration>());
    }

    public override void Configure(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["CurrentGreetingMessage"] = "Current Greeting: {0}",
            ["NewGreetingMessage"] = "New Greeting: {0}"
        };

        configurationBuilder.AddInMemoryCollection(appSettings);
    }

    public override void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        serviceCollection.AddScoped<ITestService, TestService>();
    }

    private void UpdateGreeting(
        string newGreeting,
        ITestService testService,
        IConfiguration configuration)
    {
        string unformattedCurrentGreeting = configuration.GetValue<string>("CurrentGreetingMessage");
        string unformattedNewGreeting = configuration.GetValue<string>("NewGreetingMessage");

        string currentGreeting = testService.GetGreeting();

        Console.WriteLine(unformattedCurrentGreeting, currentGreeting);

        string updatedGreeting = testService.UpdateGreeting(newGreeting);

        Console.WriteLine(unformattedNewGreeting, updatedGreeting);
    }
}
```

Add Automation Console Builder in startup

```csharp
class Program
{
    static void Main(string[] args)
    {
        IAutomationConsoleBuilder builder =
            // Passing in "args" is optional
            AutomationConsole.CreateDefaultBuilder(args)
                // Add command name/path here
                .AddCommand<TestCommand>("greet");

        IAutomationConsole console = builder.Build();

        console.Run();
    }
}
```

Run command

```dotnetcli
dotnet run -- greet --greeting "Hello World"
```

## Other Resources

- [.NET Command-Line-Api Project](https://github.com/dotnet/command-line-api)
- [Tutorial: Create a .NET tool using the .NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create)
- [Tutorial: Install and use a .NET global tool using the .NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-use)
- [Tutorial: Install and use a .NET local tool using the .NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use)

## Related Packages

| Package                                                                                                                | Description                                                                     |
| ---------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| [BlazorFocused.Automation.Runtime](https://www.nuget.org/packages/BlazorFocused.Automation.Runtime/)                   | Runtime services for BlazorFocused.Automation framework                         |
| [BlazorFocused.Automation.PowerShell](https://www.nuget.org/packages/BlazorFocused.Automation.PowerShell/)             | PowerShell PSCmdlet SDK utilities for Automation framework development          |
| [BlazorFocused.Automation.PowerShell.Tools](https://www.nuget.org/packages/BlazorFocused.Automation.PowerShell.Tools/) | Development tools for running/testing PSCmdlets built with Automation framework |
