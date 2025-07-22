using LiteDB;
using System.Linq.Expressions;

namespace ServerShared.Database;

/// <summary>
/// Database Connection with LiteDB.
/// </summary>
/// <typeparam name="T">Any type.</typeparam>
public class DataBaseConnection<T>
{
    internal bool _disposed;
    internal readonly LiteDatabase DB;
    internal readonly ILiteCollection<T> Collection;

    /// <summary>
    /// Create a Database and Collection with type <typeparamref name="T"/> name.
    /// </summary>
    public DataBaseConnection() : this(typeof(T).Name) { }

    /// <summary>
    /// Create a Database with <paramref name="dbName"/> as Database Name and Collection with type <typeparamref name="T"/> name.
    /// </summary>
    /// <param name="dbName">Database Name</param>
    public DataBaseConnection(string dbName) : this(dbName, typeof(T).Name) { }

    /// <summary>
    /// Create a Database with <paramref name="dbName"/> as Database Name and Collection with <paramref name="collectionName"/>.
    /// </summary>
    /// <param name="dbName">Database Name.</param>
    /// <param name="collectionName">The Collection Name.</param>
    public DataBaseConnection(string dbName, string collectionName) : this(dbName, collectionName, null) { }

    /// <summary>
    /// Create a Database with <paramref name="dbName"/> as Database Name and Collection with <paramref name="collectionName"/>.
    /// </summary>
    /// <param name="dbName">Database Name.</param>
    /// <param name="collectionName">The Collection Name.</param>
    /// <param name="password">The password for the database.</param>
    public DataBaseConnection(string dbName, string collectionName, string? password)
    {
        // Ensuring the Dabase is exists.
        Directory.CreateDirectory("Database");
        DB = new LiteDatabase(new ConnectionString() { Connection = ConnectionType.Shared, Filename = Path.Combine("Database", $"{dbName}.db") , Password = password });
        Collection = DB.GetCollection<T>(collectionName);
    }

    /// <summary>
    /// Create <paramref name="data"/> inside <see cref="Collection"/>.
    /// </summary>
    /// <param name="data"></param>
    public virtual void Create(T data)
    {
        Collection.Insert(data);
    }

    /// <summary>
    /// Updating/Inserting <paramref name="data"/> inside <see cref="Collection"/>.
    /// </summary>
    /// <param name="data"></param>
    /// <remarks>
    /// If <paramref name="data"/> is null, it returns before updating!
    /// </remarks>
    public virtual void Update(T? data)
    {
        if (data == null)
            return;
        Collection.Upsert(data);
    }

    /// <summary>
    /// Get a <see cref="List{T}"/> with what <paramref name="predicate"/> finds.
    /// </summary>
    /// <param name="predicate">Predicate.</param>
    /// <returns><see cref="List{T}"/> with the found <typeparamref name="T"/> objects.</returns>
    public virtual List<T> GetList(Expression<Func<T, bool>> predicate)
    {
        return [.. Collection.Find(predicate)];
    }

    /// <summary>
    /// Get One element with the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Predicate.</param>
    /// <returns><see langword="null"/> if not found otherwise the found <typeparamref name="T"/> object.</returns>
    public virtual T? GetOne(Expression<Func<T, bool>> predicate)
    {
        return Collection.FindOne(predicate);
    }

    /// <summary>
    /// Check if the <paramref name="predicate"/> exists.
    /// </summary>
    /// <param name="predicate">Predicate.</param>
    /// <returns>Returns <see langword="true"/> if found.</returns>
    public virtual bool Exists(Expression<Func<T, bool>> predicate)
    {
        return Collection.Exists(predicate);
    }

    /// <summary>
    /// Delete many object with the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Predicate.</param>
    public virtual void Delete(Expression<Func<T, bool>> predicate)
    {
        Collection.DeleteMany(predicate);
    }

    /// <summary>
    /// Getting a value by reference and saving the value when disposing.
    /// </summary>
    /// <param name="predicate">Predicate.</param>
    /// <returns></returns>
    public virtual DBRefValue<T>? GetRefValue(Expression<Func<T, bool>> predicate)
    {
        return new(Collection.FindOne(predicate), this);
    }

    /// <summary>
    /// Close Database.
    /// </summary>
    public virtual void Close()
    {
        DB.Dispose();
        _disposed = true;
    }

    /// <summary>
    /// Is Database closed.
    /// </summary>
    /// <returns></returns>
    public bool IsClosed()
        => _disposed;
}
