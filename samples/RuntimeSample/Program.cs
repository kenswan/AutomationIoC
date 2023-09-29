// -------------------------------------------------------
// Copyright (c) BlazorFocused All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RuntimeSample.Attributes;
using RuntimeSample.Services;
using RuntimeSample.Session;
using BlazorFocused.Automation.Runtime;

namespace RuntimeSample;

internal class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Enter '1' for default startup");
            Console.WriteLine("Enter '2' for launch arguments startup");
            Console.WriteLine("Enter '3' for attribute binding startup");
            Console.WriteLine("Enter any other key (or press enter) to exit");

            string userOption = Console.ReadLine();

            IReportOrchestrator reportOrchestrator = userOption switch
            {
                "1" => RunAutomatedReportWithDefaultStartup(),
                "2" => RunAutomatedReportWithLaunchArguments(args),
                "3" => RunAutomatedReportWithAttributeBinding(),
                _ => null
            };

            if (reportOrchestrator is null)
            {
                break;
            }

            reportOrchestrator.CompileReport();

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            Console.Clear();
        }
    }

    private static IReportOrchestrator RunAutomatedReportWithDefaultStartup()
    {
        using IHost host = AutomationRuntime.GenerateRuntimeHost<Startup>();
        IServiceProvider services = host.Services;

        return services.GetRequiredService<IReportOrchestrator>();
    }

    private static IReportOrchestrator RunAutomatedReportWithLaunchArguments(string[] args)
    {
        Console.WriteLine("Launch setting arguments will be used at this time");

        var startup = new Startup();

        using IHost host = AutomationRuntime.GenerateRuntimeHost(
            startup.Configure,
            startup.ConfigureServices,
            args,
            startup.GenerateParameterConfigurationMapping());

        IServiceProvider services = host.Services;
        return services.GetRequiredService<IReportOrchestrator>();
    }

    private static IReportOrchestrator RunAutomatedReportWithAttributeBinding()
    {
        var reportOrchestrator = new ReportOrchestratorWithAttributes();
        var reportServiceCache = new ReportSession();

        AutomationRuntime
            .BindServicesByAttribute<ReportDependencyAttribute, Startup>(
                reportServiceCache,
                reportOrchestrator);

        return reportOrchestrator;
    }
}
