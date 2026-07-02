using System.Text.Json;
using OptiLauncher.Core.Enums;
using OptiLauncher.Services.Interfaces;

namespace OptiLauncher.Services.Implementations;

/// <summary>
/// Manages application settings persistence using JSON file storage.
/// </summary>
public class SettingsService : ISettingsService
{
    private readonly ILoggingService _logger;
    private readonly string _settingsPath;
    private SettingsData _data = new();
    
    public ThemeType Theme
    {
        get => _data.Theme;
        set => _data.Theme = value;
    }
    
    public string Language
    {
        get => _data.Language;
        set => _data.Language = value;
    }
    
    public bool AnimationsEnabled
    {
        get => _data.AnimationsEnabled;
        set => _data.AnimationsEnabled = value;
    }
    
    public string AccentColor
    {
        get => _data.AccentColor;
        set => _data.AccentColor = value;
    }
    
    public double Transparency
    {
        get => _data.Transparency;
        set => _data.Transparency = Math.Clamp(value, 0.0, 1.0);
    }
    
    public bool AutoUpdate
    {
        get => _data.AutoUpdate;
        set => _data.AutoUpdate = value;
    }
    
    public bool LaunchAfterLogin
    {
        get => _data.LaunchAfterLogin;
        set => _data.LaunchAfterLogin = value;
    }
    
    public bool CloseAfterLaunch
    {
        get => _data.CloseAfterLaunch;
        set => _data.CloseAfterLaunch = value;
    }
    
    public bool MinimizeInsteadOfClose
    {
        get => _data.MinimizeInsteadOfClose;
        set => _data.MinimizeInsteadOfClose = value;
    }
    
    public int DownloadThreads
    {
        get => _data.DownloadThreads;
        set => _data.DownloadThreads = Math.Clamp(value, 1, 16);
    }
    
    public long NetworkSpeedLimit
    {
        get => _data.NetworkSpeedLimit;
        set => _data.NetworkSpeedLimit = Math.Max(0, value);
    }
    
    public int MemoryCache
    {
        get => _data.MemoryCache;
        set => _data.MemoryCache = Math.Clamp(value, 64, 8192);
    }
    
    public string? JavaPath
    {
        get => _data.JavaPath;
        set => _data.JavaPath = value;
    }
    
    public string? JvmArguments
    {
        get => _data.JvmArguments;
        set => _data.JvmArguments = value;
    }
    
    public int MaxRamMB
    {
        get => _data.MaxRamMB;
        set => _data.MaxRamMB = Math.Clamp(value, 512, 65536);
    }
    
    public bool AutoDetectJava
    {
        get => _data.AutoDetectJava;
        set => _data.AutoDetectJava = value;
    }
    
    public string? GameDirectory
    {
        get => _data.GameDirectory;
        set => _data.GameDirectory = value;
    }
    
    public bool Fullscreen
    {
        get => _data.Fullscreen;
        set => _data.Fullscreen = value;
    }
    
    public int WindowWidth
    {
        get => _data.WindowWidth;
        set => _data.WindowWidth = Math.Clamp(value, 640, 7680);
    }
    
    public int WindowHeight
    {
        get => _data.WindowHeight;
        set => _data.WindowHeight = Math.Clamp(value, 480, 4320);
    }
    
    public string CpuPriority
    {
        get => _data.CpuPriority;
        set => _data.CpuPriority = value;
    }

    /// <summary>
    /// Initializes a new instance of the SettingsService.
    /// </summary>
    /// <param name="logger">The logging service for recording operations.</param>
    public SettingsService(ILoggingService logger)
    {
        _logger = logger;
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var optiLauncherDir = Path.Combine(appData, "OptiLauncher");
        Directory.CreateDirectory(optiLauncherDir);
        _settingsPath = Path.Combine(optiLauncherDir, "settings.json");
    }

    /// <inheritdoc/>
    public async Task SaveAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_settingsPath, json);
            _logger.Debug("Settings saved successfully to {Path}", _settingsPath);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to save settings to {Path}", _settingsPath);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task LoadAsync()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = await File.ReadAllTextAsync(_settingsPath);
                _data = JsonSerializer.Deserialize<SettingsData>(json) ?? new SettingsData();
                _logger.Information("Settings loaded from {Path}", _settingsPath);
            }
            else
            {
                _data = new SettingsData();
                _logger.Information("No settings file found, using defaults");
                await SaveAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to load settings from {Path}, using defaults", _settingsPath);
            _data = new SettingsData();
        }
    }

    /// <inheritdoc/>
    public void ResetToDefaults()
    {
        _data = new SettingsData();
        _logger.Information("Settings reset to defaults");
    }

    /// <inheritdoc/>
    public async Task ExportSettingsAsync(string filePath)
    {
        try
        {
            var json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
            _logger.Information("Settings exported to {Path}", filePath);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to export settings to {Path}", filePath);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task ImportSettingsAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                _logger.Warning("Import file not found: {Path}", filePath);
                throw new FileNotFoundException("Settings file not found", filePath);
            }
            
            var json = await File.ReadAllTextAsync(filePath);
            var imported = JsonSerializer.Deserialize<SettingsData>(json) ?? new SettingsData();
            
            // Validate imported data
            imported.DownloadThreads = Math.Clamp(imported.DownloadThreads, 1, 16);
            imported.MaxRamMB = Math.Clamp(imported.MaxRamMB, 512, 65536);
            imported.Transparency = Math.Clamp(imported.Transparency, 0.0, 1.0);
            
            _data = imported;
            await SaveAsync();
            _logger.Information("Settings imported from {Path}", filePath);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to import settings from {Path}", filePath);
            throw;
        }
    }

    /// <summary>
    /// Internal data class for JSON serialization of settings.
    /// </summary>
    private class SettingsData
    {
        public ThemeType Theme { get; set; } = ThemeType.Dark;
        public string Language { get; set; } = "en";
        public bool AnimationsEnabled { get; set; } = true;
        public string AccentColor { get; set; } = "#0078D4";
        public double Transparency { get; set; } = 0.85;
        public bool AutoUpdate { get; set; } = true;
        public bool LaunchAfterLogin { get; set; } = false;
        public bool CloseAfterLaunch { get; set; } = true;
        public bool MinimizeInsteadOfClose { get; set; } = false;
        public int DownloadThreads { get; set; } = 8;
        public long NetworkSpeedLimit { get; set; } = 0;
        public int MemoryCache { get; set; } = 512;
        public string? JavaPath { get; set; } = null;
        public string? JvmArguments { get; set; } = null;
        public int MaxRamMB { get; set; } = 4096;
        public bool AutoDetectJava { get; set; } = true;
        public string? GameDirectory { get; set; } = null;
        public bool Fullscreen { get; set; } = false;
        public int WindowWidth { get; set; } = 854;
        public int WindowHeight { get; set; } = 480;
        public string CpuPriority { get; set; } = "Normal";
    }
}