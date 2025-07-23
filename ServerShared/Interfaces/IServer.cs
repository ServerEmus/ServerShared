namespace ServerShared.Interfaces;

public interface IServer
{
    /// <summary>
    /// Server Id
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// Start the server
    /// </summary>
    /// <returns>'true' if the server was successfully started, 'false' if the server failed to start</returns>
    bool Start();

    /// <summary>
    /// Stop the server
    /// </summary>
    /// <returns>'true' if the server was successfully stopped, 'false' if the server is already stopped</returns>
    bool Stop();
}