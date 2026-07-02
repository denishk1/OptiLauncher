using Microsoft.Extensions.DependencyInjection;
using OptiLauncher.Services.Implementations;
using OptiLauncher.Services.Interfaces;

namespace OptiLauncher.Services.DependencyInjection;

/// <summary>
/// Extension methods for registering OptiLauncher services in the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all OptiLauncher services for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddOptiLauncherServices(this IServiceCollection services)
    {
        // Logging
        services.AddSingleton<ILoggingService, LoggingService>();
        
        // Settings
        services.AddSingleton<ISettingsService, SettingsService>();
        
        return services;
    }
}