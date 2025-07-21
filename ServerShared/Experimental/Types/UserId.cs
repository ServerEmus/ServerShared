#if DEBUG
namespace ServerShared.Experimental.Types;

public class UserId
{
    public UserId() : this(Guid.Empty) { }

    public UserId(Guid guid)
    {
        id = guid;
    }

    private Guid id;

    public override bool Equals(Object obj)
    {
        if (obj == null)
            return false;

        UserId uid = obj as UserId;
        if ((Object)uid == null)
            return false;

        return id == uid.id;
    }

    public bool Equals(UserId uid)
    {
        if ((object)uid == null)
            return false;

        return id == uid.id;
    }

    public override int GetHashCode()
        => id.GetHashCode();

    public byte[] ToByteArray()
        => id.ToByteArray();

    public override string ToString()
        => id.ToString();

    public static bool operator ==(UserId a, UserId b)
    {
        if (Object.ReferenceEquals(a, b))
            return true;

        if (((object)a == null) || ((object)b == null))
            return false;

        return a.id == b.id;
    }

    public static bool operator !=(UserId a, UserId b)
    {
        return !(a == b);
    }

    public static implicit operator Guid(UserId uid)
        => uid.id;

    public static implicit operator UserId(Guid id)
        => new(id);

    public static UserId New()
        => new(Guid.NewGuid());
}
#endif