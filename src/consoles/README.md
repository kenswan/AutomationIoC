[![Nuget Version](https://img.shields.io/nuget/v/AutomationIoC?logo=nuget)](https://www.nuget.org/packages/AutomationIoC)
[![Nuget Downloads](https://img.shields.io/nuget/dt/AutomationIoC?logo=nuget)](https://www.nuget.org/packages/AutomationIoC)
[![Continuous Integration](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml/badge.svg)](https://github.com/kenswan/AutomationIoC/actions/workflows/continuous-integration.yml)

# AutomationIoC.Consoles

Dependency Injection Framework for C# Console Applications

## Requirements

- [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) or [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Getting Started

### Installation

```dotnetcli
dotnet add package AutomationIoC.Consoles
```

### Development

_See [Sample Project](https://github.com/kenswan/AutomationIoC/tree/main/sample) for a full sample solution_

Create AutomationIoC Consoles Command to run

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
            serviceBinderFactory.Create<ITestService>(),
            serviceBinderFactory.Create<IConfiguration>());
    }

    public override Action<IConfigurationBuilder> ConfigurationBuilder => (configurationBuilder) =>
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["CurrentGreetingMessage"] = "Current Greeting: {0}",
            ["NewGreetingMessage"] = "New Greeting: {0}"
        };
    };

    public override Action<IServiceCollection> Services => (serviceCollection) =>
    {
        serviceCollection.AddScoped<ITestService, TestService>();
    };

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

Add AutomationIoC Consoles Builder in startup

```csharp
class Program
{
    static void Main(string[] args)
    {
        IAutomationIoConsoleBuilder builder =
            AutomationIoConsole.CreateDefaultBuilder(args)
                // Add command name/path here
                .AddCommand<TestCommand>("greet");

        IAutomationIoConsole console = builder.Build();

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

## Related Packages

| Package                                                                                       | Description                                                                     |
| --------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| [AutomationIoC](https://www.nuget.org/packages/AutomationIoC)                                 | Framework Runtime for AutomationIoC Console Applications and PowerShell Cmdlets |
| [AutomationIoC.PSCmdlets](https://www.nuget.org/packages/AutomationIoC.PSCmdlets)             | Dependency Injection Framework for C# PowerShell Cmdlets                        |
| [AutomationIoC.PSCmdlets.Tools](https://www.nuget.org/packages/AutomationIoC.PSCmdlets.Tools) | Tooling for running/testing C# PowerShell Cmdlets                               |
