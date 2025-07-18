using ServerCore.Models;
using ServerCore.Models.App;
using ServerCore.Models.Auth;
using ServerCore.Models.JWTToken;
using ServerCore.Models.Requests;

namespace ServerShared.Database;

/// <summary>
/// Database Connection Manager.
/// </summary>
public static class DBManager
{
    /// <summary>
    /// Database Connection for <see cref="ServerCore.Models.App.AppApi"/>.
    /// </summary>
    public static DataBaseConnection<AppApi> AppAPI { get; }


    static DBManager()
    {
        AppAPI = new("App");
    }
}
