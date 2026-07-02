using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;
using OptiLauncher.Services.Interfaces;

namespace OptiLauncher.Services.Implementations;

/// <summary>
/// Implements logging functionality using Serilog for file and console output.
/// </summary>
public class LoggingService : ILoggingService, IDisposable
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes the logging service with file and console sinks.
    /// </summary>
    public LoggingService()
    {
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "OptiLauncher",
            "logs",
            "optilauncher-.log"
        );

        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.File(
                logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();
    }

    /// <inheritdoc/>
    public void Information(string message, params object[] args) => 
        _logger.Information(message, args);

    /// <inheritdoc/>
    public void Warning(string message, params object[] args) => 
        _logger.Warning(message, args);

    /// <inheritdoc/>
    public void Error(string message, params object[] args) => 
        _logger.Error(message, args);

    /// <inheritdoc/>
    public void Error(Exception exception, string message, params object[] args) => 
        _logger.Error(exception, message, args);

    /// <inheritdoc/>
    public void Debug(string message, params object[] args) => 
        _logger.Debug(message, args);

    /// <summary>
    /// Disposes the underlying Serilog logger.
    /// </summary>
    public void Dispose()
    {
        Log.CloseAndFlush();
    }
}