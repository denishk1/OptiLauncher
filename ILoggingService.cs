namespace OptiLauncher.Services.Interfaces;

/// <summary>
/// Provides centralized logging functionality for OptiLauncher.
/// </summary>
public interface ILoggingService
{
    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="args">Optional format arguments.</param>
    void Information(string message, params object[] args);
    
    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="args">Optional format arguments.</param>
    void Warning(string message, params object[] args);
    
    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="args">Optional format arguments.</param>
    void Error(string message, params object[] args);
    
    /// <summary>
    /// Logs an error with an exception.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="args">Optional format arguments.</param>
    void Error(Exception exception, string message, params object[] args);
    
    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="args">Optional format arguments.</param>
    void Debug(string message, params object[] args);
}