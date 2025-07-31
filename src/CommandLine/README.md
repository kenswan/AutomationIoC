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
public class TestCommand : IAutomationCommand
{
    public void Initialize(AutomationCommand command)
    {
        Option<string> configurationKeyOption = new(name: "--key")
        {
            Description = "Name of key to pull from configuration."
        };

        command.Options.Add(configurationKeyOption);

        command.SetAction(async (parseResult, context, cancellationToken) =>
        {
            string configurationKey = parseResult.GetValue(configurationKeyOption);

            IServiceProvider serviceProvider = context.ServiceProvider;

            TestConfigurationService testConfigurationService =
                serviceProvider.GetRequiredService<TestConfigurationService>();

            string configurationValue =
                await testConfigurationService
                    .GetConfigurationValueAsync(configurationKey, cancellationToken)
                    .ConfigureAwait(false);

            Console.WriteLine($"Configuration Value Requested: {configurationValue}");
        });
    }
}

// Sample test configuration service to retrieve configuration values
public class TestConfigurationService(IConfiguration configuration)
{
    public string GetConfigurationValue(string key) =>
        configuration.GetValue<string>(key) ?? throw new KeyNotFoundException($"Configuration key '{key}' not found.");

    public Task<string> GetConfigurationValueAsync(string key, CancellationToken cancellationToken = default) =>
        Task.FromResult(GetConfigurationValue(key));
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
                .AddCommand<TestCommand>("check-config")
                // Build IConfiguration
                .Configure((context, configurationBuilder) =>
                {
                    var appSettings = new Dictionary<string, string>
                    {
                        {
                            "MyConfigurationKey", Guid.NewGuid().ToString()
                        }
                    };
                    configurationBuilder.AddInMemoryCollection(appSettings);
                })
                // Register services
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<TestConfigurationService>();
                });

        IAutomationConsole console = builder.Build();

        console.Run();
    }
}
```

Run command

```dotnetcli
dotnet run -- check-config --key MyConfigurationKey
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
