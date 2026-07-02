using System.Security.Cryptography;
using System.Text;

namespace OptiLauncher.Utils.Helpers;

/// <summary>
/// Provides security-related utilities for encryption and hashing.
/// </summary>
public static class SecurityHelper
{
    private static readonly byte[] EntropyBytes = Encoding.UTF8.GetBytes("OptiLauncher_Secure_Storage_v1");

    /// <summary>
    /// Encrypts a string using Windows Data Protection API.
    /// </summary>
    /// <param name="data">The plain text data to encrypt.</param>
    /// <returns>Base64-encoded encrypted data.</returns>
    public static string EncryptString(string data)
    {
        var plainBytes = Encoding.UTF8.GetBytes(data);
        var encryptedBytes = System.Security.Cryptography.ProtectedData.Protect(
            plainBytes, 
            EntropyBytes, 
            System.Security.Cryptography.DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// Decrypts a string using Windows Data Protection API.
    /// </summary>
    /// <param name="encryptedData">Base64-encoded encrypted data.</param>
    /// <returns>The decrypted plain text.</returns>
    public static string DecryptString(string encryptedData)
    {
        var encryptedBytes = Convert.FromBase64String(encryptedData);
        var plainBytes = System.Security.Cryptography.ProtectedData.Unprotect(
            encryptedBytes, 
            EntropyBytes, 
            System.Security.Cryptography.DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(plainBytes);
    }

    /// <summary>
    /// Computes the SHA1 hash of a file.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <returns>Hex-encoded SHA1 hash.</returns>
    public static async Task<string> ComputeSha1HashAsync(string filePath)
    {
        using var sha1 = SHA1.Create();
        await using var stream = File.OpenRead(filePath);
        var hash = await sha1.ComputeHashAsync(stream);
        return Convert.ToHexStringLower(hash);
    }

    /// <summary>
    /// Generates a cryptographically secure random string.
    /// </summary>
    /// <param name="length">The length of the random string.</param>
    /// <returns>A random string for use as tokens or identifiers.</returns>
    public static string GenerateRandomString(int length = 32)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var bytes = RandomNumberGenerator.GetBytes(length);
        var result = new StringBuilder(length);
        foreach (var b in bytes)
        {
            result.Append(chars[b % chars.Length]);
        }
        return result.ToString();
    }
}