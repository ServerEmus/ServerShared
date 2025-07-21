using ServerShared.Database;
using ServerShared.UserModels;

namespace ServerShared.Controllers;

public static class UserCommonController
{
    public static bool IsUserBanned(Guid UserId)
    {
        var user = DBManager.UserCommon.GetOne(x => x.UserId == UserId);
        if (user == null)
            return false;
        return user.IsBanned;
    }

    public static bool IsUserExist(Guid UserId)
    {
        return DBManager.UserCommon.Exists(x => x.UserId == UserId);
    }

    public static string GetUserName(Guid UserId)
    {
        var user = DBManager.UserCommon.GetOne(x => x.UserId == UserId);
        if (user == null)
            return string.Empty;
        return user.Name;
    }

    public static IEnumerable<Guid> GetFriendIds(Guid UserId)
    {
        var user = DBManager.UserCommon.GetOne(x => x.UserId == UserId);
        if (user == null)
            return [];
        return user.Friends;
    }

    public static void RemoveFromFriends(Guid UserId, Guid FriendId)
    {
        UserCommon? user = DBManager.UserCommon.GetOne(x => x.UserId == UserId);
        if (user != null)
        {
            user.Friends.Remove(FriendId);
            DBManager.UserCommon.Update(user);
        }
        UserCommon? friend = DBManager.UserCommon.GetOne(x => x.UserId == FriendId);
        if (friend != null)
        {
            friend.Friends.Remove(UserId);
            DBManager.UserCommon.Update(friend);
        }
        DBManager.UserFriend.Delete(x => x.UserId == UserId && x.IdOfFriend == FriendId);
        DBManager.UserFriend.Delete(x => x.UserId == FriendId && x.IdOfFriend == UserId);
    }
}
