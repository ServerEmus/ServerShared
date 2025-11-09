using ServerShared.CommonModels;

namespace ServerShared.EventArguments;

/// <summary>
/// An event argument that represent a server model.
/// </summary>
/// <param name="model">The server model.</param>
public class ServerModelEventArgs(ServerModel model) : EventArgs
{
    /// <summary>
    /// The server model that sent.
    /// </summary>
    public ServerModel Model => model;
}
