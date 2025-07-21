using ServerShared.Database;
using ServerShared.UserModels;

namespace ServerShared.Controller;

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
        return DBManager.UserCommon.GetOne(x => x.UserId == UserId) != null;
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
    }
}
