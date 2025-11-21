using ModdableWebServer.Interfaces;

namespace ServerShared.EventArguments;

/// <summary>
/// An event argument that represent the session and the received bytes.
/// </summary>
/// <param name="session">The session the data received from.</param>
/// <param name="data">The bytes that received.</param>
public class SessionBytesReceivedEventArgs(ISession session, ReadOnlySpan<byte> data) : EventArgs
{
    /// <summary>
    /// The session the data received from.
    /// </summary>
    public ISession Session = session;

    /// <summary>
    /// The bytes that received.
    /// </summary>
    public Memory<byte> Data = data.ToArray();
}
