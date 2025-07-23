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

    bool IsSSL { get; }

    bool IsClosed { get; }
}