using ModdableWebServer.Interfaces;

namespace ServerShared.EventArguments;

/// <summary>
/// An event argument that represent the session and the session's Id.
/// </summary>
/// <param name="session">The session.</param>
/// <param name="sessionGuid">The session guid.</param>
public class SessionGuidEventArgs(ISession session, Guid sessionGuid) : EventArgs
{
    /// <summary>
    /// The session.
    /// </summary>
    public ISession Session = session;

    /// <summary>
    /// The sessions Guid.
    /// </summary>
    public Guid Id = sessionGuid;
}
