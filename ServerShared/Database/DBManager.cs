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
    /// Database Connection for <see cref="ApiModels.AppApi"/>.
    /// </summary>
    public static DataBaseConnection<AppApi> AppApi { get; }
    #endregion
    #region Common
    /// <summary>
    /// Database Connection for <see cref="AuthModels.CurrentToken"/>.
    /// </summary>
    public static DataBaseConnection<CurrentToken> CurrentToken { get; }
    /// <summary>
    /// Database Connection for <see cref="AuthModels.Demux"/>.
    /// </summary>
    public static DataBaseConnection<Demux> DemuxConnection { get; }
    #endregion
    #region Common
    /// <summary>
    /// Database Connection for <see cref="CommonModels.CDKey"/>.
    /// </summary>
    public static DataBaseConnection<CDKey> CDKey { get; }
    #endregion
    #region Product
    /// <summary>
    /// Database Connection for <see cref="ProductModels.ProductBranch"/>.
    /// </summary>
    public static DataBaseConnection<ProductBranch> Branch { get; }
    /// <summary>
    /// Database Connection for <see cref="ProductModels.ProductStore"/>.
    /// </summary>
    public static DataBaseConnection<ProductStore> Store { get; }
    /// <summary>
    /// Database Connection for <see cref="ProductModels.ProductConfig"/>.
    /// </summary>
    public static DataBaseConnection<ProductConfig> ProductConfig { get; }
    #endregion
    #region User
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserActivity"/>.
    /// </summary>
    public static DataBaseConnection<UserActivity> UserActivity { get; }
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserCloudSave"/>.
    /// </summary>
    public static DataBaseConnection<UserCloudSave> UserCloudSave { get; }
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserCommon"/>.
    /// </summary>
    public static DataBaseConnection<UserCommon> UserCommon { get; }
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserFriend"/>.
    /// </summary>
    public static DataBaseConnection<UserFriend> UserFriend { get; }
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserGameSession"/>.
    /// </summary>
    public static DataBaseConnection<UserGameSession> UserGameSession { get; }
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserLogin"/>.
    /// </summary>
    public static DataBaseConnection<UserLogin> UserLogin { get; }
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserOwnership"/>.
    /// </summary>
    public static DataBaseConnection<UserOwnership> UserOwnership { get; }
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserOwnershipBasic"/>.
    /// </summary>
    public static DataBaseConnection<UserOwnershipBasic> UserOwnershipBasic { get; }
    /// <summary>
    /// Database Connection for <see cref="UserModels.UserPlaytime"/>.
    /// </summary>
    public static DataBaseConnection<UserPlaytime> UserPlaytime { get; }
    #endregion

    static DBManager()
    {
        AppApi = new("App");

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
