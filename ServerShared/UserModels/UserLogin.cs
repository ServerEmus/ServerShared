namespace ServerShared.UserModels;

/// <summary>
/// User Login data.
/// </summary>
public class UserLogin : UserBase
{
    /// <summary>
    /// User's Email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's Password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// User's Date of Birth.
    /// </summary>
    public string DateOfBirth { get; set; } = "2000-01-01";

    /// <summary>
    /// User's Name on Uplay.
    /// </summary>
    public string NameOnPlatform { get; set; } = string.Empty;

    /// <summary>
    /// Country the user registered to.
    /// </summary>
    public string Country { get; set; } = "EU";

    /// <summary>
    /// User's Prefered Language
    /// </summary>
    public string PreferredLanguage { get; set; } = "US";

    /// <summary>
    /// Some random shit.
    /// </summary>
    public string LegalOptinsKey { get; set; } = string.Empty;
}
