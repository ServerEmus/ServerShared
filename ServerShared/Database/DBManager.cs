using ServerShared.ApiModels;
using ServerShared.AuthModels;
using ServerShared.CommonModels;
using ServerShared.ProductModels;
using ServerShared.UserModels;

namespace ServerShared.Database;

/// <summary>
/// Database Connection Manager.
/// </summary>
public static class DBManager
{
#region API
    /// <summary>
    /// Database Connection for <see cref="ServerShared.Models.App.AppApi"/>.
    /// </summary>
    public static DataBaseConnection<AppApi> AppAPI { get; }
#endregion
#region Common
    public static DataBaseConnection<CurrentToken> CurrentToken { get; }
    public static DataBaseConnection<Demux> DemuxConnection { get; }
#endregion
#region Common
    public static DataBaseConnection<CDKey> CDKey { get; }
#endregion
#region Product
    public static DataBaseConnection<ProductBranch> Branch { get; }
    public static DataBaseConnection<ProductStore> Store { get; }
    public static DataBaseConnection<ProductConfig> ProductConfig { get; }
#endregion
#region User
    public static DataBaseConnection<UserActivity> UserActivity { get; }
    public static DataBaseConnection<UserCloudSave> UserCloudSave { get; }
    public static DataBaseConnection<UserCommon> UserCommon { get; }
    public static DataBaseConnection<UserFriend> UserFriend { get; }
    public static DataBaseConnection<UserGameSession> UserGameSession { get; }
    public static DataBaseConnection<UserLogin> UserLogin { get; }
    public static DataBaseConnection<UserOwnership> UserOwnership { get; }
    public static DataBaseConnection<UserOwnershipBasic> UserOwnershipBasic { get; }
    public static DataBaseConnection<UserPlaytime> UserPlaytime { get; }
#endregion

    static DBManager()
    {
        AppAPI = new("App");

        CurrentToken = new("tmp");
        DemuxConnection = new("tmp");

        CDKey = new("Common");

        Branch = new("Product");
        Store = new("Product");
        ProductConfig = new("Product");

        UserActivity = new("User");
        UserCloudSave = new("User");
        UserCommon = new("User");
        UserFriend = new("User");
        UserGameSession = new("User");
        UserLogin = new("User");
        UserOwnership = new("User");
        UserOwnershipBasic = new("User");
        UserPlaytime = new("User");
    }
}
