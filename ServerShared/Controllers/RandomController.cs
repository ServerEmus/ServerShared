namespace ServerShared.Controllers;

/// <summary>
/// Randomization Controller
/// </summary>
public static class RandomController
{
    /// <summary>
    /// Shared <see cref="Random"/>.
    /// </summary>
    public static Random RNG { get; } = new();

    /// <summary>
    /// Get a random string with the Length as <paramref name="lenght"/>.
    /// </summary>
    /// <param name="lenght">String Length</param>
    /// <param name="chars">Characters to generate a string from.</param>
    /// <returns>A random string</returns>
    public static string RandomString(int lenght, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
    {
        string ran = string.Empty;

        for (int i = 0; i < lenght; i++)
        {
            int x = RNG.Next(chars.Length);
            ran += chars[x];
        }

        return ran;
    }

    /// <summary>
    /// Generate a random Uplay-like CD Key.
    /// </summary>
    /// <returns>A cd key.</returns>
    public static string RandomCDKey()
    {
        return $"{RandomString(3)}-{RandomString(4)}-{RandomString(4)}-{RandomString(4)}-{RandomString(4)}";
    }
}
