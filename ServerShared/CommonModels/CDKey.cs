namespace ServerShared.CommonModels;

/// <summary>
/// Represent a Uplay-Like CD Key.
/// </summary>
public class CDKey
{
    /// <summary>
    /// What product this <see cref="Key"/> tied to.
    /// </summary>
    [LiteDB.BsonId]
    public uint ProductId { get; set; }
    /// <summary>
    /// The actual CD Key.
    /// </summary>
    public string Key { get; set; } = string.Empty;
}
