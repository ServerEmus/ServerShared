using System.Text.Json;

namespace ServerShared.Controllers;

/// <summary>
/// A Controller for reading, writing json files.
/// </summary>
public static class JsonController
{
    /// <summary>
    /// A Json Serializer option for easier access.
    /// </summary>
    public static JsonSerializerOptions SerializerOptions { get; } = new()
    {
        WriteIndented = true,
        IndentCharacter = '\t',
        IndentSize = 1,
    };

    /// <summary>
    /// Reading and creating a <typeparamref name="T"/> from <paramref name="fileName"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T Read<T>(string fileName) where T : new()
    {
        T? instance = new();
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Configs" , fileName);
        if (File.Exists(filePath))
            instance = JsonSerializer.Deserialize<T>(File.ReadAllText(filePath));
        instance ??= new();
        File.WriteAllText(filePath, JsonSerializer.Serialize(instance, SerializerOptions));
        return instance;
    }

    /// <summary>
    /// Saving a <paramref name="value"/> to <paramref name="fileName"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="fileName"></param>
    public static void Save<T>(T value, string fileName) where T : new()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Configs", fileName);
        File.WriteAllText(filePath, JsonSerializer.Serialize(value, SerializerOptions));
    }
}
