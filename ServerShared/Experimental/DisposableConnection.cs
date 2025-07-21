using ServerShared.Database;

namespace ServerShared.Experimental;

internal class DisposableConnection<T>(T value, DataBaseConnection<T> con) : IDisposable
{
    private T val = value;
    private readonly DataBaseConnection<T> dbcon = con;

    public ref T Value => ref val;

    ~DisposableConnection()
    {
        Dispose(disposing: false);
    }

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