namespace ServerShared;

public enum TokenType
{
    None,
    Orbit,
    Token,
    Ticket,

    AuthToken,
    UbiV1 = AuthToken,
    UplayPCV1,
    RememberMe_v1,
    BasicAuth,
    // This token only used to just say the current UserId
    UserIdToken,
}

/// <summary>
/// Flag for the product.
/// </summary>
[Flags]
public enum AppFlags
{
    /// <summary>
    /// Product is no longer available.
    /// </summary>
    NotAvailable,
    /// <summary>
    /// Product can be downloaded.
    /// </summary>
    Downloadable,
    /// <summary>
    /// Product is in a playable state.
    /// </summary>
    Playable,
    /// <summary>
    /// Product has a Denuvo Demo.
    /// </summary>
    DenuvoForceTimeTrial,
    /// <summary>
    /// Product has Denuvo.
    /// </summary>
    Denuvo,
    /// <summary>
    /// Product is from a subscription.
    /// </summary>
    FromSubscription,
    /// <summary>
    /// Product is from an expired subscription.
    /// </summary>
    FromExpiredSubscription
}