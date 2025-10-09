namespace ServerShared.IO;

/// <summary>
/// 
/// </summary>
public interface IBigSerializable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer">The stream to write data to.</param>
    public void Serialize(BinaryWriterBig writer);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader">The stream to read data to.</param>
    public void Deserialize(BinaryReaderBig reader);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Any type to serialize.</typeparam>
public interface IBigInstanceSerializable<T> : IBigSerializable
{
    /// <summary>
    /// Parses a <paramref name="reader"/> into a value.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReaderBig"/> to parse.</param>
    /// <returns>The result of parsing <paramref name="reader"/>.</returns>
    public abstract static T Parse(BinaryReaderBig reader);
}