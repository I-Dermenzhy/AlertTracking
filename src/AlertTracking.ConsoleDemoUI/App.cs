using AlertTracking.Abstractions.Monitors;
using AlertTracking.Domain.Dtos;

using Microsoft.Extensions.Configuration;

using System.Text;

namespace AlertTracking.ConsoleDemoUI;

internal sealed class App
{
    private readonly IRegionAlertMonitor _alertMonitor;
    private readonly IConfiguration _configuration;

    public App(IRegionAlertMonitor alertMonitor, IConfiguration configuration)
    {
        _alertMonitor = alertMonitor ?? throw new ArgumentNullException(nameof(alertMonitor));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        _alertMonitor.RegionAlertCheckedWhileTracking += (sender, args) => LogAlertStatusToConsole(args);
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.OutputEncoding = Encoding.Unicode;

        DisplayHelloMessage();
        DisplayRegionChoice();
        string chosenRegionName = HandleUserRegionChoice();

        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        Task trackingTask = _alertMonitor.StartTrackingAsync(chosenRegionName, 1000, cancellationToken);

        Console.ReadKey();
        cancellationTokenSource.Cancel();

        try
        {
            await trackingTask;
        }
        catch
        {
            LogExceptionToConsole(trackingTask);
        }

        await Task.Delay(TimeSpan.FromMilliseconds(100));

        await RunAsync();
    }

    private static void DisplaySeparator() => Console.WriteLine(string.Join("", Enumerable.Repeat(".", 55)));
    private static void DisplayHelloMessage() => Console.WriteLine("Welcome to the alert tracking app!");

    private static void LogAlertStatusToConsole(RegionAlertArgs args)
    {
        string regionName = args.RegionName;
        bool isAlert = args.IsAlert;

        string logMessage = isAlert
            ? $"There is an air alert in {regionName}!\nLocal time: {DateTime.Now:HH:mm:ss}\n"
            : $"There is no air alerts in {regionName}.\nLocal time: {DateTime.Now:HH:mm:ss}\n";

        Console.ForegroundColor = isAlert
            ? ConsoleColor.DarkRed
            : ConsoleColor.Green;

        Console.Clear();
        Console.WriteLine(logMessage);

        Console.ResetColor();

        Console.WriteLine();
        Console.Write("Press any key to stop tracking: ");
    }

    private static void LogExceptionToConsole(Task task) => LogExceptionToConsole(task.Exception!);

    private static void LogExceptionToConsole(Exception ex) => LogExceptionToConsole($"There was an exception: {ex.Message}");

    private static void LogExceptionToConsole(string logMessage)
    {
        DisplaySeparator();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(logMessage);
        Console.ResetColor();
    }

    private void DisplayRegionChoice()
    {
        string[] regionNames = GetRegionNames();

        DisplaySeparator();
        Console.WriteLine("Enter the number to choose a region to track:");

        for (int i = 1; i < regionNames.Length; i++)
            Console.WriteLine($"{i} - {regionNames[i]}");

        Console.WriteLine("Anything other - exit app");
    }

    private string[] GetRegionNames()
    {
        var regionIdsSection = _configuration.GetSection("UkraineAlertApi:RegionIds");

        return regionIdsSection.GetChildren()
            .Select(r => r.Key)
            .ToArray();
    }

    private string HandleUserRegionChoice()
    {
        try
        {
            DisplaySeparator();
            Console.Write("My choice: ");

            int chosenNumber = int.Parse(Console.ReadLine()!);

            if (chosenNumber is < 1 or > 24)
                Environment.Exit(0);

            string[] regionNames = GetRegionNames();

            return regionNames[chosenNumber];
        }
        catch
        {
            Environment.Exit(0);
            return OnInvalidConsoleInput();
        }
    }

    private string OnInvalidConsoleInput()
    {
        DisplaySeparator();
        Console.WriteLine("Invalid input!");
        Console.WriteLine("Try again");

        Thread.Sleep(2000);
        Console.Clear();

        DisplayRegionChoice();
        return HandleUserRegionChoice();
    }
}
