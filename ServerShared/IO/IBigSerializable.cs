namespace ServerShared.IO;

/// <summary>
/// Serializable interface for big stream writer/reader.
/// </summary>
public interface IBigSerializable
{
    /// <summary>
    /// Serialize <see langword="this"/> into <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The stream to write data to.</param>
    public void Serialize(BinaryWriterBig writer);

    /// <summary>
    /// Read <see langword="this"/> from <paramref name="reader"/>. 
    /// </summary>
    /// <param name="reader">The stream to read data to.</param>
    public void Deserialize(BinaryReaderBig reader);
}

/// <summary>
/// Serializable interface for getting <typeparamref name="T"/> with big stream writer/reader.
/// </summary>
/// <typeparam name="T">Any type to serialize.</typeparam>
public interface IBigInstanceSerializable<T> : IBigSerializable
{
    /// <summary>
    /// Parses a <paramref name="reader"/> into a value.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReaderBig"/> to parse.</param>
    /// <returns>The result of parsing <paramref name="reader"/>.</returns>
    public abstract static T? Parse(BinaryReaderBig reader);
}