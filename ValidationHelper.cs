using System.Text.RegularExpressions;

namespace OptiLauncher.Utils.Helpers;

/// <summary>
/// Provides validation utilities for launcher inputs and data.
/// </summary>
public static partial class ValidationHelper
{
    [GeneratedRegex(@"^[a-zA-Z0-9_\-\.\s]+$")]
    private static partial Regex InstanceNameRegex();
    
    [GeneratedRegex(@"^[a-zA-Z]:[\\\/](?:[a-zA-Z0-9\s_\-\.]+[\\\/])*[a-zA-Z0-9\s_\-\.]*$")]
    private static partial Regex WindowsPathRegex();

    /// <summary>
    /// Validates if a string is a valid instance name.
    /// </summary>
    /// <param name="name">The instance name to validate.</param>
    /// <returns>True if valid; otherwise false.</returns>
    public static bool IsValidInstanceName(string? name)
    {
        return !string.IsNullOrWhiteSpace(name) && 
               name.Length <= 100 && 
               InstanceNameRegex().IsMatch(name);
    }

    /// <summary>
    /// Validates if a path is valid on Windows.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <returns>True if valid; otherwise false.</returns>
    public static bool IsValidWindowsPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;
        
        return WindowsPathRegex().IsMatch(path) || 
               Uri.TryCreate(path, UriKind.Absolute, out _);
    }

    /// <summary>
    /// Validates if a URL is valid.
    /// </summary>
    /// <param name="url">The URL to validate.</param>
    /// <returns>True if valid; otherwise false.</returns>
    public static bool IsValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    /// <summary>
    /// Validates if a Minecraft version string is valid.
    /// </summary>
    /// <param name="version">The version string to validate.</param>
    /// <returns>True if valid; otherwise false.</returns>
    public static bool IsValidMinecraftVersion(string? version)
    {
        return !string.IsNullOrWhiteSpace(version) && 
               version.Length <= 50 && 
               version.All(c => char.IsLetterOrDigit(c) || c == '.' || c == '-' || c == '_');
    }

    /// <summary>
    /// Validates a Java version number.
    /// </summary>
    /// <param name="version">The Java version to validate.</param>
    /// <returns>True if the version is supported; otherwise false.</returns>
    public static bool IsSupportedJavaVersion(int version)
    {
        return version is 8 or 11 or 17 or 21;
    }

    /// <summary>
    /// Sanitizes a filename to remove invalid characters.
    /// </summary>
    /// <param name="filename">The filename to sanitize.</param>
    /// <returns>A sanitized filename.</returns>
    public static string SanitizeFilename(string filename)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Join("_", filename.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries))
            .TrimEnd('.');
    }
}