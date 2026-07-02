using OptiLauncher.Core.Enums;

namespace OptiLauncher.Services.Interfaces;

/// <summary>
/// Manages application settings persistence and retrieval.
/// </summary>
public interface ISettingsService
{
    /// <summary>Gets or sets the current UI theme.</summary>
    ThemeType Theme { get; set; }
    
    /// <summary>Gets or sets the current language code.</summary>
    string Language { get; set; }
    
    /// <summary>Gets or sets whether UI animations are enabled.</summary>
    bool AnimationsEnabled { get; set; }
    
    /// <summary>Gets or sets the accent color in hex format.</summary>
    string AccentColor { get; set; }
    
    /// <summary>Gets or sets the transparency level (0.0 to 1.0).</summary>
    double Transparency { get; set; }
    
    /// <summary>Gets or sets whether automatic updates are enabled.</summary>
    bool AutoUpdate { get; set; }
    
    /// <summary>Gets or sets whether to launch the game immediately after login.</summary>
    bool LaunchAfterLogin { get; set; }
    
    /// <summary>Gets or sets whether to close the launcher after game launch.</summary>
    bool CloseAfterLaunch { get; set; }
    
    /// <summary>Gets or sets whether to minimize instead of closing.</summary>
    bool MinimizeInsteadOfClose { get; set; }
    
    /// <summary>Gets or sets the number of parallel download threads.</summary>
    int DownloadThreads { get; set; }
    
    /// <summary>Gets or sets the network speed limit in bytes per second (0 for unlimited).</summary>
    long NetworkSpeedLimit { get; set; }
    
    /// <summary>Gets or sets the memory cache size in MB.</summary>
    int MemoryCache { get; set; }
    
    /// <summary>Gets or sets the path to the Java executable.</summary>
    string? JavaPath { get; set; }
    
    /// <summary>Gets or sets custom JVM arguments.</summary>
    string? JvmArguments { get; set; }
    
    /// <summary>Gets or sets the maximum RAM allocation in MB.</summary>
    int MaxRamMB { get; set; }
    
    /// <summary>Gets or sets whether to auto-detect Java installations.</summary>
    bool AutoDetectJava { get; set; }
    
    /// <summary>Gets or sets the custom game directory path.</summary>
    string? GameDirectory { get; set; }
    
    /// <summary>Gets or sets whether to launch in fullscreen mode.</summary>
    bool Fullscreen { get; set; }
    
    /// <summary>Gets or sets the game window width.</summary>
    int WindowWidth { get; set; }
    
    /// <summary>Gets or sets the game window height.</summary>
    int WindowHeight { get; set; }
    
    /// <summary>Gets or sets the CPU priority for the game process.</summary>
    string CpuPriority { get; set; }

    /// <summary>Saves the current settings to persistent storage.</summary>
    Task SaveAsync();
    
    /// <summary>Loads settings from persistent storage.</summary>
    Task LoadAsync();
    
    /// <summary>Resets all settings to their default values.</summary>
    void ResetToDefaults();
    
    /// <summary>Exports settings to a specified file path.</summary>
    Task ExportSettingsAsync(string filePath);
    
    /// <summary>Imports settings from a specified file path.</summary>
    Task ImportSettingsAsync(string filePath);
}