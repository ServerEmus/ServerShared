using ServerShared.Database;
using System.Linq.Expressions;

namespace ServerShared.Experimental;

internal class DisposableDataBaseConnection<T> : DataBaseConnection<T>
{
    internal virtual DisposableConnection<T>? GetDispableConnection(Expression<Func<T, bool>> predicate)
    {
        return new(Collection.FindOne(predicate), this);
    }
}
