using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using OptiLauncher.Services.DependencyInjection;
using OptiLauncher.Views;

namespace OptiLauncher;

/// <summary>
/// Main application class for OptiLauncher responsible for initialization and DI setup.
/// </summary>
public class App : Application
{
    /// <summary>
    /// Gets the application service provider for dependency injection.
    /// </summary>
    public static IServiceProvider Services { get; private set; } = null!;

    /// <summary>
    /// Initializes the application, sets up DI container, and loads services.
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();
    }

    /// <summary>
    /// Called when the application framework initialization is complete.
    /// Creates and shows the main window.
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Configures dependency injection services for the application.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddOptiLauncherServices();
    }
}