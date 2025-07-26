[![Nuget Version](https://img.shields.io/nuget/v/AutomationIoC.CommandLine?logo=nuget)](https://www.nuget.org/packages/AutomationIoC.CommandLine)
[![Nuget Downloads](https://img.shields.io/nuget/dt/AutomationIoC.CommandLine?logo=nuget)](https://www.nuget.org/packages/AutomationIoC.CommandLine)
[![Continuous Integration](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml)

# AutomationIoC.Runtime

Dependency Injection Framework for C# Console Applications

## NuGet Packages

| Package                                                                                | Description                                                |
| -------------------------------------------------------------------------------------- | ---------------------------------------------------------- |
| [AutomationIoC.CommandLine](https://www.nuget.org/packages/AutomationIoC.CommandLine/) | Dependency Injection Framework for C# Console Applications |

## Documentation

Please visit the [AutomationIoC Documentation Site](https://kenswan.github.io/AutomationIoC/) for installation, getting started, and API documentation.

## Samples

Please visit and/or download our [AutomationIoC.CommandLine Sample Solution](https://github.com/kenswan/AutomationIoC/tree/main/samples/CommandLineSample) to get a more in-depth view of usage.

## Installation

.NET CLI

```dotnetcli

dotnet add package AutomationIoC.CommandLine

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

| Package                                                                                          | Description                                                                     |
| ------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------- |
| [AutomationIoC.Runtime](https://www.nuget.org/packages/AutomationIoC.Runtime/)                   | Runtime services for AutomationIoC framework                                    |
| [AutomationIoC.PowerShell](https://www.nuget.org/packages/AutomationIoC.PowerShell/)             | PowerShell PSCmdlet SDK utilities for Automation framework development          |
| [AutomationIoC.PowerShell.Tools](https://www.nuget.org/packages/AutomationIoC.PowerShell.Tools/) | Development tools for running/testing PSCmdlets built with Automation framework |
