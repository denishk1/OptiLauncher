using System.Globalization;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Localization;
using OptiLauncher.Core.Enums;

namespace OptiLauncher.Localization.Services;

/// <summary>
/// Provides localization services for OptiLauncher with support for multiple languages.
/// </summary>
public class LocalizationService : IStringLocalizer
{
    private readonly Dictionary<string, Dictionary<string, string>> _localizations = new();
    private string _currentLanguage = "en";
    
    /// <summary>
    /// Event raised when the current language changes.
    /// </summary>
    public event EventHandler? LanguageChanged;

    /// <summary>
    /// Gets the currently active language code.
    /// </summary>
    public string CurrentLanguage
    {
        get => _currentLanguage;
        private set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                LanguageChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Initializes the localization service and loads all language resources.
    /// </summary>
    public LocalizationService()
    {
        LoadAllResources();
    }

    /// <summary>
    /// Gets a localized string by key for the current language.
    /// </summary>
    /// <param name="name">The localization key.</param>
    /// <returns>The localized string or the key if not found.</returns>
    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, value == null);
        }
    }

    /// <summary>
    /// Gets a formatted localized string by key for the current language.
    /// </summary>
    /// <param name="name">The localization key.</param>
    /// <param name="arguments">Arguments for string formatting.</param>
    /// <returns>The formatted localized string.</returns>
    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var value = GetString(name);
            if (value == null)
                return new LocalizedString(name, name, true);
            
            try
            {
                return new LocalizedString(name, string.Format(value, arguments), false);
            }
            catch
            {
                return new LocalizedString(name, value, false);
            }
        }
    }

    /// <summary>
    /// Gets all localized strings for a specific culture.
    /// </summary>
    /// <param name="includeParentCultures">Whether to include parent culture strings.</param>
    /// <returns>Collection of localized strings.</returns>
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        if (_localizations.TryGetValue(_currentLanguage, out var strings))
        {
            return strings.Select(kvp => new LocalizedString(kvp.Key, kvp.Value, false));
        }
        return Enumerable.Empty<LocalizedString>();
    }

    /// <summary>
    /// Changes the current language and notifies subscribers.
    /// </summary>
    /// <param name="languageCode">The language code to switch to (e.g., "en", "ru", "uk").</param>
    public void SetLanguage(string languageCode)
    {
        if (_localizations.ContainsKey(languageCode))
        {
            CurrentLanguage = languageCode;
            CultureInfo.CurrentUICulture = new CultureInfo(languageCode);
        }
    }

    /// <summary>
    /// Gets a list of all available language codes.
    /// </summary>
    /// <returns>Array of language codes.</returns>
    public string[] GetAvailableLanguages() => _localizations.Keys.ToArray();

    /// <summary>
    /// Gets the display name for a language code.
    /// </summary>
    /// <param name="languageCode">The language code.</param>
    /// <returns>The display name of the language.</returns>
    public string GetLanguageDisplayName(string languageCode) => languageCode switch
    {
        "en" => "English",
        "ru" => "Русский",
        "uk" => "Українська",
        _ => languageCode
    };

    private string? GetString(string key)
    {
        if (_localizations.TryGetValue(_currentLanguage, out var strings))
        {
            if (strings.TryGetValue(key, out var value))
                return value;
        }
        
        // Fallback to English
        if (_currentLanguage != "en" && _localizations.TryGetValue("en", out var enStrings))
        {
            if (enStrings.TryGetValue(key, out var enValue))
                return enValue;
        }
        
        return null;
    }

    private void LoadAllResources()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(r => r.Contains("Resources.") && r.EndsWith(".json"));

        foreach (var resourceName in resourceNames)
        {
            // Extract language code from filename (e.g., Strings.en.json -> en)
            var parts = resourceName.Split('.');
            if (parts.Length >= 3)
            {
                var langCode = parts[^2]; // Get the language code part
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    var json = reader.ReadToEnd();
                    var strings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (strings != null)
                    {
                        _localizations[langCode] = strings;
                    }
                }
            }
        }
    }
}