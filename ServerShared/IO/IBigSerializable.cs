namespace ServerShared.IO;

/// <summary>
/// 
/// </summary>
public interface IBigSerializable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    public void Serialize(BinaryWriterBig stream);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    public void Deserialize(BinaryReaderBig stream);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBigInstanceSerializable<T> : IBigSerializable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public abstract static T Parse(BinaryReaderBig reader);
}