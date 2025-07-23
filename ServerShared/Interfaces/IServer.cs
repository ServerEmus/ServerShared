namespace ServerShared.Interfaces;

public interface IServer
{
    Guid Id { get; }
    bool Start();
    bool Stop();
}