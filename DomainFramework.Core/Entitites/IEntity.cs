namespace DomainFramework.Core
{
    public interface IEntity
    {
        object GetId();

        object GetData();
    }

    public interface IEntity<K, T> : IEntity
    {
        K Id { get; }

        T Data { get; }
    }
}