namespace ServerShared.IO;

/// <summary>
/// Serializable interface for endianness stream writer/reader.
/// </summary>
public interface ICustomSerializable
{
    /// <summary>
    /// Serialize <see langword="this"/> into <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The stream to write data to.</param>
    public void Serialize(EndiannessWriter writer);

    /// <summary>
    /// Read <see langword="this"/> from <paramref name="reader"/>. 
    /// </summary>
    /// <param name="reader">The stream to read data to.</param>
    public void Deserialize(EndiannessReader reader);
}

/// <summary>
/// Serializable interface for getting <typeparamref name="T"/> with endianness writer/reader.
/// </summary>
/// <typeparam name="T">Any type to serialize.</typeparam>
public interface ICustomInstanceSerializable<T> : ICustomSerializable
{
    /// <summary>
    /// Parses a <paramref name="reader"/> into a value.
    /// </summary>
    /// <param name="reader">The <see cref="EndiannessReader"/> to parse.</param>
    /// <returns>The result of parsing <paramref name="reader"/>.</returns>
    public abstract static T? Parse(EndiannessReader reader);
}