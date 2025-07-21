using ServerShared.Database;
using ServerShared.CommonModels;
using ServerShared.UserModels;
using System.Text;
using System.Text.Json;

namespace ServerShared.Controllers;

public static class CloudSaveController
{
    public static byte[] GetSave(Guid UserId, string itemOrName, uint productId, out bool UseBinaryOctetStream)
    {
        UseBinaryOctetStream = false;
        if (!OwnershipController.IsOwned(UserId, productId))
            return [];
        if (itemOrName.Contains("all"))
        {
            var cloudsaves = DBManager.UserCloudSave.GetList(x => x.UserId == UserId && x.UplayId == productId);
            if (cloudsaves == null)
            {
                cloudsaves = new();
                UserCloudSave save = new()
                {
                    UplayId = productId,
                    UserId = UserId
                };
                DBManager.UserCloudSave.Create(save);
                cloudsaves.Add(save);
            }
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cloudsaves));
        }
        else
        {
            var cloudSave = GetCloudSave(UserId, itemOrName, productId);
            if (cloudSave == null)
                return [];
            var path = Path.Combine(ServerConfig.Instance.Demux.ServerFilesPath, cloudSave.UserId.ToString(), cloudSave.UplayId.ToString(), cloudSave.SaveName);
            if (!File.Exists(path))
                return [];
            UseBinaryOctetStream = true;
            return File.ReadAllBytes(path);
        }
    }

    public static bool PutSave(Guid UserId, string itemOrName, uint productId, byte[] body)
    {
        if (!OwnershipController.IsOwned(UserId, productId))
            return false;
        var cloudSave = GetCloudSave(UserId, itemOrName, productId);
        if (cloudSave == null)
            return false;
        var path = Path.Combine(ServerConfig.Instance.Demux.ServerFilesPath, cloudSave.UserId.ToString(), cloudSave.UplayId.ToString(), cloudSave.SaveName);
        if (!Directory.Exists(Path.GetDirectoryName(path)))
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, body);
        return true;
    }

    public static bool DeleteSave(Guid UserId, uint itemId, uint productId)
    {
        if (!OwnershipController.IsOwned(UserId, productId))
            return false;
        var cloudSave = GetCloudSave(UserId, itemId.ToString(), productId);
        if (cloudSave == null)
            return false;
        DBManager.UserCloudSave.Delete(x => x.UserId == UserId && x.SaveId == itemId && x.UplayId == productId);
        var path = Path.Combine(ServerConfig.Instance.Demux.ServerFilesPath, cloudSave.UserId.ToString(), cloudSave.UplayId.ToString(), cloudSave.SaveName);
        if (Directory.Exists(Path.GetDirectoryName(path)) && File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }

    public static UserCloudSave? GetCloudSave(Guid UserId, string itemOrName, uint productId)
    {
        UserCloudSave? cloudsave = null;
        if (itemOrName.Contains("savegame"))
        {
            cloudsave = DBManager.UserCloudSave.GetOne(x => x.UserId == UserId && x.SaveName == itemOrName && x.UplayId == productId);
            if (cloudsave == null)
            {
                cloudsave = new()
                {
                    UserId = UserId,
                    UplayId = productId,
                    SaveId = 0,
                    SaveName = itemOrName
                };
                DBManager.UserCloudSave.Create(cloudsave);
            }
        }
        else
        {
            if (!uint.TryParse(itemOrName, out uint itemId))
                return cloudsave;
            cloudsave = DBManager.UserCloudSave.GetOne(x => x.UserId == UserId && x.SaveId == itemId && x.UplayId == productId);
            if (cloudsave == null)
            {
                cloudsave = new()
                {
                    UserId = UserId,
                    UplayId = productId,
                    SaveId = itemId,
                    SaveName = itemId + ".savegame"
                };
                DBManager.UserCloudSave.Create(cloudsave);
            }
        }
        return cloudsave;
    }
}
