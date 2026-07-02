using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace OptiLauncher;

/// <summary>
/// Main entry point for the OptiLauncher application.
/// </summary>
internal sealed class Program
{
    /// <summary>
    /// Initializes and runs the OptiLauncher application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    /// <summary>
    /// Configures the Avalonia application builder with necessary dependencies and styling.
    /// </summary>
    /// <returns>The configured AppBuilder instance.</returns>
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}