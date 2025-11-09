namespace ServerShared.Database;

/// <summary>
/// An <see cref="IDisposable"/> for able to edit values more easily.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="value">The value</param>
/// <param name="con"></param>
public sealed class DBRefValue<T>(T value, DataBaseConnection<T> con) : IDisposable
{
    private T val = value;
    private readonly DataBaseConnection<T> dbcon = con;

    /// <summary>
    /// Get the value by reference.
    /// </summary>
    public ref T Value => ref val;

    /// <summary>
    /// Finalizer
    /// </summary>
    ~DBRefValue()
    {
        Dispose(disposing: false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing && val != null && !dbcon.IsClosed())
            dbcon.Update(val);
    }
}