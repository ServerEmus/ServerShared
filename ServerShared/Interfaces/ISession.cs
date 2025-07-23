namespace ServerShared.Interfaces;

public interface ISession
{
    /// <summary>
    /// Session Id
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Server
    /// </summary>
    IServer GetServer();

    /// <summary>
    /// Is the session connected?
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Is session is SSL not HTTP.
    /// </summary>
    bool IsSSL { get; }

    /// <summary>
    /// Session is closed.
    /// </summary>
    bool IsClosed { get; }
}